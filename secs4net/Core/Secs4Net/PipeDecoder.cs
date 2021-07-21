using Microsoft.Toolkit.HighPerformance;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PipeDecoder
    {
        private readonly PipeReader _reader;
        private readonly PipeWriter _input;
        private readonly Channel<MessageHeader> _controlMessageChannel = Channel
            .CreateUnbounded<MessageHeader>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true,
                AllowSynchronousContinuations = true,
            });

        private readonly Channel<SecsMessage> _dataMessageChannel = Channel
            .CreateBounded<SecsMessage>(new BoundedChannelOptions(capacity: 32)
            {
                SingleReader = false,
                SingleWriter = true,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait,
            });

        public PipeDecoder(PipeReader reader, PipeWriter input)
        {
            _reader = reader;
            _input = input;
        }

        internal IAsyncEnumerable<MessageHeader> GetControlMessages(CancellationToken cancellation)
            => _controlMessageChannel.Reader.ReadAllAsync(cancellation);

        public IAsyncEnumerable<SecsMessage> GetDataMessages(CancellationToken cancellation)
            => _dataMessageChannel.Reader.ReadAllAsync(cancellation);

        internal PipeWriter Input => _input;

        public async Task StartAsync(CancellationToken cancellation)
        {
            var reader = _reader;
            var dataMessageWriter = _dataMessageChannel.Writer;
            var stack = new Stack<List<Item>>(capacity: 8);
            var totalLengthBytes = new byte[4];
            var messageHeaderBytes = new byte[10];
            ReadOnlySequence<byte> buffer;
            while (true)
            {
            Start:
                // 0: get total message length 4 bytes
                buffer = await EnsureBufferAsync(required: 4, cancellation: cancellation).ConfigureAwait(false);
                var totalLengthSeq = buffer.Slice(buffer.Start, 4);
                totalLengthSeq.CopyTo(totalLengthBytes);
                uint messageLength = BinaryPrimitives.ReadUInt32BigEndian(totalLengthBytes);
                reader.AdvanceTo(totalLengthSeq.End);

                Trace.WriteLine($"Get new message with length: {messageLength}");

                // 1: get message header 10 bytes
                buffer = await EnsureBufferAsync(required: 10, cancellation: cancellation).ConfigureAwait(false);
                var messageHaderSeq = buffer.Slice(buffer.Start, 10);
                messageHaderSeq.CopyTo(messageHeaderBytes);
                var header = MessageHeader.Decode(messageHeaderBytes);
                Trace.WriteLine($"Get message(id:{header.SystemBytes}) header");

                if (messageLength == 10) // only message header
                {
                    reader.AdvanceTo(messageHaderSeq.End);
                    if (header.MessageType == MessageType.DataMessage)
                    {
                        await ProduceDataMessageAsync(dataMessageWriter, header, item: null, cancellation).ConfigureAwait(false);
                    }
                    else
                    {
                        await _controlMessageChannel.Writer.WriteAsync(header, cancellation).ConfigureAwait(false);
                    }
                    continue;
                }

                var remainedBuffer = buffer.Slice(messageHaderSeq.End);
                if (remainedBuffer.Length >= messageLength - 10)
                {
                    Trace.WriteLine($"Get data message({header.SystemBytes}) with total bytes: {messageLength} and decoded directly");
                    var completedItem = Item.DecodeFromFullBuffer(ref remainedBuffer);
                    reader.AdvanceTo(remainedBuffer.End);
                    await ProduceDataMessageAsync(dataMessageWriter, header, completedItem, cancellation).ConfigureAwait(false);
                    continue;
                }
                reader.AdvanceTo(messageHaderSeq.End);

            GetNewItem:
                // 2: get _format + _lengthByteCount(2bit) 1 byte
                buffer = await EnsureBufferAsync(required: 1, cancellation: cancellation).ConfigureAwait(false);
                Item.DecodeFormatAndLengthByteCount(buffer.FirstSpan.DangerousGetReferenceAt(0), out var itemFormat, out var itemContentLengthByteCount);
                reader.AdvanceTo(buffer.GetPosition(1));

                // 3: get _itemLength bytes(size= _lengthByteCount), at most 3 byte
                buffer = await EnsureBufferAsync(required: itemContentLengthByteCount, cancellation: cancellation).ConfigureAwait(false);
                var itemContentLengthBytes = buffer.Slice(0, itemContentLengthByteCount);
                Item.DecodeDataLength(itemContentLengthBytes, out var itemContentLength);
                reader.AdvanceTo(itemContentLengthBytes.End);

                // 4: get item content
                Item item;
                if (itemFormat == SecsFormat.List)
                {
                    if (itemContentLength == 0)
                    {
                        item = Item.L();
                        Trace.WriteLine($"Decoded List[0]");
                    }
                    else
                    {
                        Trace.WriteLine($"Decoded List[{itemContentLength}]");
                        stack.Push(new List<Item>(itemContentLength));
                        goto GetNewItem;
                    }
                }
                else
                {
                    buffer = await EnsureBufferAsync(required: itemContentLength, cancellation: cancellation).ConfigureAwait(false);
                    var itemDataBytes = buffer.Slice(0, itemContentLength);
                    item = Item.DecodeDataItem(itemFormat, itemDataBytes);
                    reader.AdvanceTo(itemDataBytes.End);
                    Trace.WriteLine($"Decoded Item[{itemFormat}], length: {itemContentLength}");
                }

                if (stack.Count > 0)
                {
                    var list = stack.Peek();
                    list.Add(item);
                    while (list.Count == list.Capacity) //stack unwind when all List's Items has decoded
                    {
                        item = Item.L(stack.Pop());
                        //Trace.WriteLine($"Unwind List[{item.Count}]");
                        if (stack.Count > 0)
                        {
                            list = stack.Peek();
                            list.Add(item);
                        }
                        else
                        {
                            Trace.WriteLine($"Get data message({header.SystemBytes}) decoded by stream decoded");
                            await ProduceDataMessageAsync(dataMessageWriter, header, item, cancellation).ConfigureAwait(false);
                            goto Start;
                        }
                    }
                    goto GetNewItem;
                }
                else
                {
                    Trace.WriteLine($"Get data message({header.SystemBytes}) decoded by stream decoded");
                    await ProduceDataMessageAsync(dataMessageWriter, header, item, cancellation).ConfigureAwait(false);
                }
            }

            static ValueTask ProduceDataMessageAsync(ChannelWriter<SecsMessage> messageWriter, MessageHeader header, Item? item, CancellationToken cancellation)
                => messageWriter.WriteAsync(new SecsMessage(header.S, header.F, header.ReplyExpected)
                {
                    SecsItem = item,
                    DeviceId = header.DeviceId,
                    Id = header.SystemBytes,
                }, cancellation);
        }

        async ValueTask<ReadOnlySequence<byte>> EnsureBufferAsync(int required, CancellationToken cancellation)
        {
            while (true)
            {
                //_secsGem.StartT8Timer();
                var result = await _reader.ReadAsync(cancellation);
                //_secsGem.StopT8Timer();
                var buffer = result.Buffer;

                if (buffer.Length >= required)
                {
                    Trace.WriteLine($"Decoder readed bytes count:{buffer.Length}");
                    return buffer;
                }
                _reader.AdvanceTo(buffer.Start, buffer.End);
            }
        }
    }
}
