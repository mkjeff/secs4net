using CommunityToolkit.HighPerformance;
using System.Buffers;
using System.Buffers.Binary;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Secs4Net;

public sealed class PipeDecoder
{
    private readonly PipeReader _reader;
    public PipeWriter Input { get; }

    private readonly Channel<MessageHeader> _controlMessageChannel = Channel
        .CreateUnbounded<MessageHeader>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = true,
        });

    private readonly Channel<(MessageHeader header, Item? rootItem)> _dataMessageChannel = Channel
        .CreateUnbounded<(MessageHeader header, Item? rootItem)>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
        });

    public PipeDecoder(PipeReader reader, PipeWriter input)
    {
        _reader = reader;
        Input = input;
    }

    internal IAsyncEnumerable<MessageHeader> GetControlMessages(CancellationToken cancellation)
        => _controlMessageChannel.Reader.ReadAllAsync(cancellation);

    public IAsyncEnumerable<(MessageHeader header, Item? rootItem)> GetDataMessages(CancellationToken cancellation)
        => _dataMessageChannel.Reader.ReadAllAsync(cancellation);

    public Task StartAsync(CancellationToken cancellation)
        => DecodeLoopAsync(_controlMessageChannel.Writer, _dataMessageChannel.Writer, _reader, cancellation);

    private static async Task DecodeLoopAsync(
        ChannelWriter<MessageHeader> controlMessageWriter,
        ChannelWriter<(MessageHeader header, Item? rootItem)> dataMessageWriter,
        PipeReader reader,
        CancellationToken cancellation)
    {
        var stack = new Stack<ItemList>(capacity: 8);
        Item item;
        var totalLengthBytes = new byte[4];
        var messageHeaderBytes = new byte[10];
        // PipeReader peek first
        var buffer = await PipeReadAsync(reader, required: 4, cancellation).ConfigureAwait(false);
        while (!cancellation.IsCancellationRequested)
        {
        Start:
            // 0: get total message length 4 bytes
            if (IsBufferInsufficient(reader, ref buffer, required: 4))
            {
                buffer = await PipeReadAsync(reader, required: 4, cancellation).ConfigureAwait(false);
            }
            var totalLengthSeq = buffer.Slice(buffer.Start, 4);
            totalLengthSeq.CopyTo(totalLengthBytes);
            uint messageLength = BinaryPrimitives.ReadUInt32BigEndian(totalLengthBytes);
            buffer = buffer.Slice(totalLengthSeq.End);

            Debug.WriteLine($"Get new message with length: {messageLength}");

            // 1: get message header 10 bytes
            if (IsBufferInsufficient(reader, ref buffer, required: 10))
            {
                buffer = await PipeReadAsync(reader, required: 10, cancellation).ConfigureAwait(false);
            }
            var messageHaderSeq = buffer.Slice(buffer.Start, 10);
            messageHaderSeq.CopyTo(messageHeaderBytes);
            MessageHeader.Decode(messageHeaderBytes, out var header);
            buffer = buffer.Slice(messageHaderSeq.End);

            Debug.WriteLine($"Get message(id:{header.Id:X8}) header");

            if (messageLength == 10) // only message header
            {
                if (header.MessageType == MessageType.DataMessage)
                {
                    await dataMessageWriter.WriteAsync((header, rootItem: null), cancellation).ConfigureAwait(false);
                }
                else
                {
                    await controlMessageWriter.WriteAsync(header, cancellation).ConfigureAwait(false);
                }
                continue;
            }

            if (buffer.Length >= messageLength - 10)
            {
                var rootItem = Item.DecodeFromFullBuffer(ref buffer);

                Debug.WriteLine($"Get data message(id:{header.Id:X8}) with total bytes: {messageLength} and decoded directly");

                await dataMessageWriter.WriteAsync((header, rootItem), cancellation).ConfigureAwait(false);
                continue;
            }

        GetNewItem:
            // 2: get _format + _lengthByteCount(2bit) 1 byte
            if (IsBufferInsufficient(reader, ref buffer, required: 1))
            {
                buffer = await PipeReadAsync(reader, required: 1, cancellation).ConfigureAwait(false);
            }

            var formatSeq = buffer.Slice(0, 1);
            Item.DecodeFormatAndLengthByteCount(formatSeq, out var itemFormat, out var itemContentLengthByteCount);

            buffer = buffer.Slice(formatSeq.End);

            // 3: get _itemLength bytes(size= _lengthByteCount), at most 3 byte
            if (IsBufferInsufficient(reader, ref buffer, required: itemContentLengthByteCount))
            {
                buffer = await PipeReadAsync(reader, required: itemContentLengthByteCount, cancellation).ConfigureAwait(false);
            }
            var itemContentLengthBytes = buffer.Slice(0, itemContentLengthByteCount);
            var itemContentLength = Item.DecodeDataLength(itemContentLengthBytes);
            buffer = buffer.Slice(itemContentLengthBytes.End);

            // 4: get item content
            if (itemFormat is SecsFormat.List)
            {
                if (itemContentLength == 0)
                {
                    item = Item.L();
                    Debug.WriteLine($"Decoded List[0]");
                }
                else
                {
                    Debug.WriteLine($"Decoded List[{itemContentLength}]");
                    stack.Push(new ItemList(size: itemContentLength));
                    goto GetNewItem;
                }
            }
            else
            {
                if (IsBufferInsufficient(reader, ref buffer, required: itemContentLength))
                {
                    buffer = await PipeReadAsync(reader, required: itemContentLength, cancellation).ConfigureAwait(false);
                }
                var itemDataBytes = buffer.Slice(0, itemContentLength);
                item = Item.DecodeDataItem(itemFormat, itemDataBytes);
                buffer = buffer.Slice(itemDataBytes.End);
                Debug.WriteLine($"Decoded Item[{itemFormat}], length: {itemContentLength}");
            }

            if (stack.Count > 0)
            {
                var list = stack.Peek();
                list.Add(item);
                while (list.IsFull) //stack unwind when all List's Items has decoded
                {
                    item = Item.L(stack.Pop().Items);
                    //Trace.WriteLine($"Unwind List[{item.Count}]");
                    if (stack.Count > 0)
                    {
                        list = stack.Peek();
                        list.Add(item);
                    }
                    else
                    {
                        Debug.WriteLine($"Get data message(id:{header.Id:X8}) decoded by data chunked");
                        await dataMessageWriter.WriteAsync((header, item), cancellation).ConfigureAwait(false);
                        goto Start;
                    }
                }
                goto GetNewItem;
            }
            else
            {
                Debug.WriteLine($"Get data message(id:{header.Id:X8}) decoded by data chunked");
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
    [SkipLocalsInit]
    private static ValueTask<ReadOnlySequence<byte>> PipeReadAsync(PipeReader reader, int required, CancellationToken cancellation)
    {
        ReadOnlySequence<byte> buffer = ReadOnlySequence<byte>.Empty;
        if (PipeTryRead(reader, required, ref buffer))
        {
            return new(buffer);
        }

        return SlowPipeReadAsync(reader, required, cancellation);

        [MethodImpl(MethodImplOptions.NoInlining)]
#if NET
        [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
#endif
        static async ValueTask<ReadOnlySequence<byte>> SlowPipeReadAsync(PipeReader reader, int required, CancellationToken cancellation)
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

    private sealed class ItemList(int size)
    {
        private readonly Item[] _items = new Item[size];
        private int _current;

        public bool IsFull => _current == _items.Length;
        public void Add(Item item) => _items[_current++] = item;
        public Item[] Items => _items;
    }
}
