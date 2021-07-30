using Microsoft.Toolkit.HighPerformance;
using PooledAwait;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PipeDecoder
    {
        private readonly PipeReader _reader;
        internal PipeWriter Input { get; }

        private readonly Channel<MessageHeader> _controlMessageChannel = Channel
            .CreateUnbounded<MessageHeader>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true,
                AllowSynchronousContinuations = true,
            });

        private readonly Channel<(MessageHeader header, Item? rootItem)> _dataMessageChannel = Channel
            .CreateBounded<(MessageHeader header, Item? rootItem)>(new BoundedChannelOptions(capacity: 32)
            {
                SingleReader = true,
                SingleWriter = true,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait,
            });

        public PipeDecoder(PipeReader reader, PipeWriter input)
        {
            _reader = reader;
            Input = input;
        }

        public IAsyncEnumerable<MessageHeader> GetControlMessages(CancellationToken cancellation)
            => _controlMessageChannel.Reader.ReadAllAsync(cancellation);

        public IAsyncEnumerable<(MessageHeader header, Item? rootItem)> GetDataMessages(CancellationToken cancellation)
            => _dataMessageChannel.Reader.ReadAllAsync(cancellation);

        public async Task StartAsync(CancellationToken cancellation)
        {
            var dataMessageWriter = _dataMessageChannel.Writer;
            var stack = new Stack<List<Item>>(capacity: 8);
            var totalLengthBytes = new byte[4];
            var messageHeaderBytes = new byte[10];
            // PipeReader peek first
            var buffer = await PipeReadAsync(_reader, required: 4, cancellation).ConfigureAwait(false);
            while (true)
            {
            Start:
                // 0: get total message length 4 bytes
                if (IsBufferInsufficient(_reader, ref buffer, required: 4))
                {
                    buffer = await PipeReadAsync(_reader, required: 4, cancellation).ConfigureAwait(false);
                }
                var totalLengthSeq = buffer.Slice(buffer.Start, 4);
                totalLengthSeq.CopyTo(totalLengthBytes);
                uint messageLength = BinaryPrimitives.ReadUInt32BigEndian(totalLengthBytes);
                buffer = buffer.Slice(totalLengthSeq.End);
                Trace.WriteLine($"Get new message with length: {messageLength}");

                // 1: get message header 10 bytes
                if (IsBufferInsufficient(_reader, ref buffer, required: 10))
                {
                    buffer = await PipeReadAsync(_reader, required: 10, cancellation).ConfigureAwait(false);
                }
                var messageHaderSeq = buffer.Slice(buffer.Start, 10);
                messageHaderSeq.CopyTo(messageHeaderBytes);
                var header = MessageHeader.Decode(messageHeaderBytes);
                buffer = buffer.Slice(messageHaderSeq.End);
                Trace.WriteLine($"Get message(id:{header.Id:X8}) header");

                if (messageLength == 10) // only message header
                {
                    if (header.MessageType == MessageType.DataMessage)
                    {
                        await dataMessageWriter.WriteAsync((header, rootItem: null), cancellation).ConfigureAwait(false);
                    }
                    else
                    {
                        await _controlMessageChannel.Writer.WriteAsync(header, cancellation).ConfigureAwait(false);
                    }
                    continue;
                }

                if (buffer.Length >= messageLength - 10)
                {
                    Trace.WriteLine($"Get data message(id:{header.Id:X8}) with total bytes: {messageLength} and decoded directly");
                    var rootItem = Item.DecodeFromFullBuffer(ref buffer);
                    await dataMessageWriter.WriteAsync((header, rootItem), cancellation).ConfigureAwait(false);
                    continue;
                }

            GetNewItem:
                // 2: get _format + _lengthByteCount(2bit) 1 byte
                if (IsBufferInsufficient(_reader, ref buffer, required: 1))
                {
                    buffer = await PipeReadAsync(_reader, required: 1, cancellation).ConfigureAwait(false);
                }
#if NET
                Item.DecodeFormatAndLengthByteCount(buffer.FirstSpan.DangerousGetReferenceAt(0), out var itemFormat, out var itemContentLengthByteCount);
#else
                Item.DecodeFormatAndLengthByteCount(buffer.First.Span.DangerousGetReferenceAt(0), out var itemFormat, out var itemContentLengthByteCount);
#endif
                buffer = buffer.Slice(1);

                // 3: get _itemLength bytes(size= _lengthByteCount), at most 3 byte
                if (IsBufferInsufficient(_reader, ref buffer, required: itemContentLengthByteCount))
                {
                    buffer = await PipeReadAsync(_reader, required: itemContentLengthByteCount, cancellation).ConfigureAwait(false);
                }
                var itemContentLengthBytes = buffer.Slice(0, itemContentLengthByteCount);
                Item.DecodeDataLength(itemContentLengthBytes, out var itemContentLength);
                buffer = buffer.Slice(itemContentLengthBytes.End);

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
                    if (IsBufferInsufficient(_reader, ref buffer, required: itemContentLength))
                    {
                        buffer = await PipeReadAsync(_reader, required: itemContentLength, cancellation).ConfigureAwait(false);
                    }
                    var itemDataBytes = buffer.Slice(0, itemContentLength);
                    item = Item.DecodeDataItem(itemFormat, itemDataBytes);
                    buffer = buffer.Slice(itemDataBytes.End);
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
                            Trace.WriteLine($"Get data message(id:{header.Id:X8}) decoded by data chunked");
                            await dataMessageWriter.WriteAsync((header, item), cancellation).ConfigureAwait(false);
                            goto Start;
                        }
                    }
                    goto GetNewItem;
                }
                else
                {
                    Trace.WriteLine($"Get data message(id:{header.Id:X8}) decoded by data chunked");
                    await dataMessageWriter.WriteAsync((header, item), cancellation).ConfigureAwait(false);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsBufferInsufficient(PipeReader reader, ref ReadOnlySequence<byte> remainedBuffer, int required)
        {
            if (remainedBuffer.Length >= required)
            {
                return false;
            }

            reader.AdvanceTo(remainedBuffer.Start);
            return !PipeTryRead(reader, required, ref remainedBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool PipeTryRead(PipeReader reader, int required, ref ReadOnlySequence<byte> buffer)
        {
            if (reader.TryRead(out var result))
            {
                buffer = result.Buffer;
                if (buffer.Length >= required)
                {
                    return true;
                }
                else
                {
                    reader.AdvanceTo(consumed: buffer.Start, examined: buffer.End);
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ValueTask<ReadOnlySequence<byte>> PipeReadAsync(PipeReader reader, int required, CancellationToken cancellation)
        {
            ReadOnlySequence<byte> buffer = ReadOnlySequence<byte>.Empty;
            if (PipeTryRead(reader, required, ref buffer))
            {
#if NET
                return ValueTask.FromResult(buffer);
#else
                return new ValueTask<ReadOnlySequence<byte>>(buffer);
#endif
            }

            return SlowPipeReadAsync(reader, required, cancellation);
            
            [MethodImpl(MethodImplOptions.NoInlining)]
            static async PooledValueTask<ReadOnlySequence<byte>> SlowPipeReadAsync(PipeReader reader, int required, CancellationToken cancellation)
            {
                while (true)
                {
                    //StartT8Timer();
                    var result = await reader.ReadAsync(cancellation).ConfigureAwait(false);
                    //StopT8Timer();
                    var buffer = result.Buffer;

                    if (buffer.Length >= required)
                    {
                        return buffer;
                    }
                    reader.AdvanceTo(consumed: buffer.Start, examined: buffer.End);
                }
            }
        }
    }
}
