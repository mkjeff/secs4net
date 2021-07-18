using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface IDecoderSource
    {
        public ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken);
    }

    internal sealed class SocketDecoderSource : IDecoderSource
    {
        private readonly Socket _socket;

        public SocketDecoderSource(Socket socket)
        {
            _socket = socket;
        }

        public ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
            => _socket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
    }

    public class MemoryDecoderSource : IDecoderSource
    {
        private readonly ReadOnlyMemory<byte> _source;
        private int _index;

        public MemoryDecoderSource(ReadOnlyMemory<byte> source)
        {
            _source = source;
        }

        public ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            var advance = Math.Min(buffer.Length, _source.Length - _index);
            _source.Slice(_index, advance).CopyTo(buffer);
            _index += advance;
            return ValueTask.FromResult(advance);
        }
    }

    public sealed class AsyncStreamDecoder
    {
        private readonly Channel<MessageHeader> _controlMessageChannel = Channel.CreateUnbounded<MessageHeader>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = true,
        });

        private readonly Channel<SecsMessage> _dataMessageChannel = Channel.CreateBounded<SecsMessage>(new BoundedChannelOptions(capacity: 20)
        {
            SingleReader = false,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
        });

        private int _bufferFilledIndex;
        private Memory<byte> _buffer;
        private readonly ISecsGem _secsGem;

        internal IAsyncEnumerable<MessageHeader> GetControlMessages(CancellationToken cancellation)
            => _controlMessageChannel.Reader.ReadAllAsync(cancellation);

        public IAsyncEnumerable<SecsMessage> GetDataMessages(CancellationToken cancellation)
            => _dataMessageChannel.Reader.ReadAllAsync(cancellation);

        public AsyncStreamDecoder(int initialBufferSize, ISecsGem secsGem)
        {
            _buffer = new byte[initialBufferSize];
            _secsGem = secsGem;
        }

        public async Task StartReceivedAsync(IDecoderSource socket, CancellationToken cancellation)
        {
            Debug.Assert(socket != null);
            var dataMessageWriter = _dataMessageChannel.Writer;
            var index = 0;
            _bufferFilledIndex = 0;
            var stack = new Stack<List<Item>>(capacity: 8);
            while (true)
            {
            Start: // 0: get total message length 4 bytes
                await EnsureBufferAsync(socket, ref index, required: 4, cancellation).ConfigureAwait(false);
                uint messageLength = GetMessageLength(_buffer.Slice(index, 4).Span);
                index += 4;
                Trace.WriteLine($"Get new message with length: {messageLength} from buffer[{index.._bufferFilledIndex}]");

                // 1: get message header 10 bytes
                await EnsureBufferAsync(socket, ref index, required: 10, cancellation).ConfigureAwait(false);
                var header = MessageHeader.Decode(_buffer.Slice(index, 10).Span);
                index += 10;
                Trace.WriteLine($"Get message(id:{header.SystemBytes}) header from buffer[{index.._bufferFilledIndex}]");

                if (messageLength == 10) // only message header
                {
                    if (header.MessageType == MessageType.DataMessage)
                    {
                        var message = new SecsMessage(header.S, header.F, replyExpected: header.ReplyExpected)
                        {
                            DeviceId = header.DeviceId,
                            Id = header.SystemBytes,
                        };
                        await dataMessageWriter.WriteAsync(message, cancellation).ConfigureAwait(false);
                    }
                    else
                    {
                        await _controlMessageChannel.Writer.WriteAsync(header, cancellation);
                    }
                    continue;
                }

                if ((_bufferFilledIndex - index) >= messageLength - 10)
                {
                    Trace.WriteLine($"Get data message({header.SystemBytes}) with total bytes: {messageLength} and decoded directly from buffer[{index.._bufferFilledIndex}]");
                    var message = new SecsMessage(header.S, header.F, header.ReplyExpected)
                    {
                        SecsItem = Item.DecodeFromFullBuffer(_buffer.Span, ref index),
                        DeviceId = header.DeviceId,
                        Id = header.SystemBytes,
                    };

                    await dataMessageWriter.WriteAsync(message, cancellation).ConfigureAwait(false);
                    continue;
                }

            // 2: get _format + _lengthByteCount(2bit) 1 byte
            GetNewItem:
                await EnsureBufferAsync(socket, ref index, required: 1, cancellation).ConfigureAwait(false);
                Item.DecodeFormatAndLengthByteCount(_buffer.Span, ref index, out var itemFormat, out var itemContentLengthByteCount);

                // 3: get _itemLength bytes(size= _lengthByteCount), at most 3 byte
                await EnsureBufferAsync(socket, ref index, required: itemContentLengthByteCount, cancellation).ConfigureAwait(false);
                Item.DecodeDataLength(_buffer.Span.Slice(index, itemContentLengthByteCount), ref index, out var itemContentLength);

                // 4: get item content
                Item item;
                if (itemFormat == SecsFormat.List)
                {
                    if (itemContentLength == 0)
                    {
                        item = Item.L();
                        Trace.WriteLine($"Decoded List[0] from buffer[{index.._bufferFilledIndex}]");
                    }
                    else
                    {
                        Trace.WriteLine($"Decoded List[{itemContentLength}] from buffer[{index.._bufferFilledIndex}]");
                        stack.Push(new List<Item>(itemContentLength));
                        goto GetNewItem;
                    }
                }
                else
                {
                    await EnsureBufferAsync(socket, ref index, required: itemContentLength, cancellation).ConfigureAwait(false);
                    item = Item.DecodeDataItem(itemFormat, _buffer.Span.Slice(index, itemContentLength));
                    Trace.WriteLine($"Decoded Item[{itemFormat}], length: {itemContentLength} from buffer[{index.._bufferFilledIndex}]");
                    index += itemContentLength;
                }

                if (stack.Count > 0)
                {
                    var list = stack.Peek();
                    list.Add(item);
                    while (list.Count == list.Capacity) //stack unwind when all Items of List has decoded
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
                            var message = new SecsMessage(header.S, header.F, header.ReplyExpected)
                            {
                                SecsItem = item,
                                DeviceId = header.DeviceId,
                                Id = header.SystemBytes,
                            };
                            await dataMessageWriter.WriteAsync(message, cancellation).ConfigureAwait(false);
                            goto Start;
                        }
                    }
                    goto GetNewItem;
                }
                else
                {
                    Trace.WriteLine($"Get data message({header.SystemBytes}) decoded by streaming decoder");
                    var message = new SecsMessage(header.S, header.F, header.ReplyExpected)
                    {
                        SecsItem = item,
                        DeviceId = header.DeviceId,
                        Id = header.SystemBytes,
                    };
                    await dataMessageWriter.WriteAsync(message, cancellation).ConfigureAwait(false);
                }
            }

            static uint GetMessageLength(Span<byte> totalLengthBufer)
            {
                totalLengthBufer.Reverse();
                var messageLength = BitConverter.ToUInt32(totalLengthBufer);
                return messageLength;
            }
        }

        ValueTask EnsureBufferAsync(IDecoderSource socket, ref int index, int required, CancellationToken cancellation)
        {
            Debug.Assert(_bufferFilledIndex >= index);
            var remainedBufferLength = _bufferFilledIndex - index;
            var need = required - remainedBufferLength;
            if (required <= remainedBufferLength)
            {
                return ValueTask.CompletedTask;
            }

            if (need > _buffer.Length)
            {
                var newSize = Math.Max(need, _buffer.Length * 2);
                Trace.WriteLine($@"<<buffer resizing>>: current size = {_buffer.Length}, new size = {newSize}, remained[{index.._bufferFilledIndex}]");

                var newBuffer = new byte[newSize];
                // keep remained data to new buffer's head
                var remained = _buffer[index.._bufferFilledIndex];
                remained.CopyTo(newBuffer);
                _buffer = newBuffer;
                index = 0;
                _bufferFilledIndex = remained.Length;
            }
            else if (need > _buffer.Length - _bufferFilledIndex)
            {
                Trace.WriteLine($@"<<buffer recyling>>: current size = {_buffer.Length}, need = {need}, remained[{index.._bufferFilledIndex}]");
                // move remained data to buffer's head
                var remained = _buffer[index.._bufferFilledIndex];
                remained.CopyTo(_buffer);
                index = 0;
                _bufferFilledIndex = remained.Length;
            }

            return SocketReceiveAsync(socket, need, cancellation);

            async ValueTask SocketReceiveAsync(IDecoderSource socket, int need, CancellationToken cancellation)
            {
                while (need > 0)
                {
                    //_secsGem.StartT8Timer();
                    var advance = await socket.ReadAsync(_buffer.Slice(_bufferFilledIndex), cancellation);
                    //_secsGem.StopT8Timer();
                    _bufferFilledIndex += advance;
                    need -= advance;

                    Trace.WriteLine($"Socket received bytes count:{advance}");
                }
            }
        }
    }
}
