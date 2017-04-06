using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static System.Diagnostics.Debug;

namespace Secs4Net
{
    /// <summary>
    ///  Stream based decoder for SECS-II message 
    /// </summary>
    public sealed class StreamDecoder
    {
        public byte[] Buffer { get; private set; }

        public int BufferOffset { get; private set; }

        public int BufferCount => Buffer.Length - BufferOffset;

        /// <summary>
        /// decoder step
        /// </summary>
        /// <param name="length"></param>
        /// <returns>(next decoder index, need byte length)</returns>
        private delegate (int step, int need) Decoder(ref int length);

        /// <summary>
        /// decode pipelines
        /// </summary>
        private readonly Decoder[] _decoders;

        private int _decoderStep;

        private readonly Action<MessageHeader,SecsMessage> _dataMsgHandler;
        private readonly Action<MessageHeader> _controlMsgHandler;

        /// <summary>
        /// Control the range of data decoder
        /// </summary>
        private int _decodeIndex;

        /// <summary>
        /// previous decoded remained count
        /// </summary>
        private int _previousRemainedCount;

        private readonly Stack<ItemListBuffer> _stack = new Stack<ItemListBuffer>();
        private uint _messageDataLength;
        private MessageHeader _msgHeader;
        private SecsFormat _format;
        private byte _lengthBits;
        private int _itemLength;

        private readonly byte[] _itemLengthBytes = new byte[4];

        public void Reset()
        {
            _stack.Clear();
            _decoderStep = 0;
            _decodeIndex = 0;
            BufferOffset = 0;
            _messageDataLength = 0;
            _previousRemainedCount = 0;
        }

        internal StreamDecoder(int streamBufferSize,
                               Action<MessageHeader> controlMsgHandler,
                               Action<MessageHeader,SecsMessage> dataMsgHandler)
        {
            Buffer = new byte[streamBufferSize];
            BufferOffset = 0;
            _decodeIndex = 0;
            _dataMsgHandler = dataMsgHandler;
            _controlMsgHandler = controlMsgHandler;
            _decoders = new Decoder[]
            {
                GetTotalMessageLength,
                GetMessageHeader,
                GetItemHeader,
                GetItemLength,
                GetItem,
            };

            (int step, int need) GetTotalMessageLength(ref int length)
            {
                var (available, need) = CheckAvailable(ref length, 4); // 4 bytes
                if (!available)
                    return (step:0, need);

                Array.Reverse(Buffer, _decodeIndex, 4);
                unsafe
                {
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref _messageDataLength), Unsafe.AsPointer(ref Buffer[_decodeIndex]), 4);
                }

                Trace.WriteLine($"Get Message Length: {_messageDataLength}");
                _decodeIndex += 4;
                length -= 4;
                return (step:1, need:0);
            }

            (int step, int need) GetMessageHeader(ref int length)
            {
                var (available, need) = CheckAvailable(ref length, 10); // 10 bytes
                if (!available)
                    return (step:1, need);

                _msgHeader = MessageHeader.Decode(Buffer, _decodeIndex);
                _decodeIndex += 10;
                _messageDataLength -= 10;
                length -= 10;
                if (_messageDataLength == 0)
                {
                    if (_msgHeader.MessageType == MessageType.DataMessage)
                    {
                        _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty));
                    }
                    else
                    {
                        _controlMsgHandler(_msgHeader);
                    }
                    return (step:0, need:0);
                }

                if (length >= _messageDataLength)
                {
                    Trace.WriteLine("Get Complete Data Message with total data");
                    _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty, BufferedDecode(Buffer, ref _decodeIndex)));
                    length -= (int) _messageDataLength;
                    _messageDataLength = 0;
                    return (step:0, need:0); //completeWith message received
                }
                return (step:2, need:0);
            }

            (int step, int need) GetItemHeader(ref int length)
            {
                var (available, need) = CheckAvailable(ref length, 1); // _format + lengthBits(2bit) 1 byte
                if (!available)
                    return (step:2, need);

                _format = (SecsFormat) (Buffer[_decodeIndex] & 0b111111_00);
                _lengthBits = (byte) (Buffer[_decodeIndex]   & 0b000000_11);
                _decodeIndex++;
                _messageDataLength--;
                length--;
                return (step:3, need:0);
            }

            (int step, int need) GetItemLength(ref int length)
            {
                var (available, need) = CheckAvailable(ref length, _lengthBits); // at most 3 byte
                if (!available)
                    return (step:3, need);

                Array.Reverse(Buffer, _decodeIndex, _lengthBits);
                unsafe
                {
                    Unsafe.CopyBlock(Unsafe.AsPointer(ref _itemLength), Unsafe.AsPointer(ref Buffer[_decodeIndex]), _lengthBits);
                }

                _itemLength = BitConverter.ToInt32(_itemLengthBytes, 0);
                Trace.WriteLineIf(_format != SecsFormat.List, $"Get format: {_format}, length: {_itemLength}");

                _decodeIndex += _lengthBits;
                _messageDataLength -= _lengthBits;
                length -= _lengthBits;
                return (step:4, need:0);
            }

            (int step, int need) GetItem(ref int length)
            {
                SecsItem item;
                if (_format == SecsFormat.List)
                {
                    if (_itemLength == 0)
                    {
                        item = Item.L();
                    }
                    else
                    {
                        _stack.Push(ItemListBuffer.Create(_itemLength));
                        return (step:2, need:0);
                    }
                }
                else
                {
                    var (available, need) = CheckAvailable(ref length, _itemLength);
                    if (!available)
                        return (step:4, need);

                    item = _itemLength == 0
                        ? _format.BytesDecode()
                        : _format.BytesDecode(Buffer, ref _decodeIndex, ref _itemLength);
                    Trace.WriteLine($"Complete Item: {_format}");

                    _decodeIndex += _itemLength;
                    _messageDataLength -= (uint) _itemLength;
                    length -= _itemLength;
                }

                if (_stack.Count == 0)
                {
                    Trace.WriteLine("Get Complete Data Message by stream decoded");
                    _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty, item));
                    return (step:0, need:0);
                }

                var list = _stack.Peek();
                list.Add(item);
                while (list.Count == list.Capacity)
                {
                    using (var buffer = _stack.Pop())
                        item = Item.L(buffer.PooledItems);

                    Trace.WriteLine($"Complete List: {item.Count}");
                    if (_stack.Count > 0)
                    {
                        list = _stack.Peek();
                        list.Add(item);
                    }
                    else
                    {
                        Trace.WriteLine("Get Complete Data Message by stream decoded");
                        _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty, item));
                        return (step:0, need:0);
                    }
                }

                return (step:2, need:0);
            }
        }

        public static SecsItem BufferedDecode(byte[] bytes, ref int index)
        {
            var format = (SecsFormat)(bytes[index] & 0xFC);
            var lengthBits = (byte)(bytes[index] & 3);
            index++;

            Array.Reverse(bytes, index, lengthBits);
            var length = 0;
            unsafe
            {
                Unsafe.CopyBlock(
                    Unsafe.AsPointer(ref length),
                    Unsafe.AsPointer(ref bytes[index]),
                    lengthBits
                );
            }

            index += lengthBits;

            if (format == SecsFormat.List)
            {
                if (length == 0)
                    return Item.L();

                using (var buffer = ItemListBuffer.Create(length))
                {
                    for (var i = 0; i < length; i++)
                        buffer.Add(BufferedDecode(bytes, ref index));
                    return Item.L(buffer.PooledItems);
                }
            }
            var item = length == 0 ? format.BytesDecode() : format.BytesDecode(bytes, ref index, ref length);
            index += length;
            return item;
        }


        private static (bool available, int need) CheckAvailable(ref int length, int required)
            => (length < required)
                ? (false, required - length)
                : (true, 0);        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">data length</param>
        /// <returns>true, if need more data to decode completed message. otherwise, return false</returns>
        public bool Decode(int length)
        {
            Assert(length > 0, "decode data length is 0.");
            var currentLength = length;
            length += _previousRemainedCount; // total available length = current length + previous remained
            var nexStep = _decoderStep;
            int need;
            do
            {
                _decoderStep = nexStep;
                (nexStep, need) = _decoders[_decoderStep](ref length);
            } while (nexStep != _decoderStep);

            Assert(_decodeIndex >= BufferOffset, "decode index should ahead of buffer index");

            var remainCount = length;
            Assert(remainCount >= 0, "remain count is only possible grater and equal zero");
            Trace.WriteLine($"remain data length: {remainCount}");
            Trace.WriteLineIf(_messageDataLength > 0, $"need data count: {need}");

            if (remainCount == 0)
            {
                if (need > Buffer.Length)
                {
                    var newSize = need * 2;
                    Trace.WriteLine($@"<<buffer resizing>>: current size = {Buffer.Length}, new size = {newSize}");

                    // increase buffer size
                    Buffer = new byte[newSize];
                }
                BufferOffset = 0;
                _decodeIndex = 0;
                _previousRemainedCount = 0;
            }
            else
            {
                BufferOffset += currentLength; // move next receive index
                var nextStepReqiredCount = remainCount + need;
                if (nextStepReqiredCount > BufferCount)
                {
                    if (nextStepReqiredCount > Buffer.Length)
                    {
                        var newSize = Math.Max(_messageDataLength/2, nextStepReqiredCount)*2;
                        Trace.WriteLine(
                                        $@"<<buffer resizing>>: current size = {Buffer.Length}, remained = {remainCount}, new size = {newSize}");

                        // out of total buffer size
                        // increase buffer size
                        var newBuffer = new byte[newSize];
                        // keep remained data to new buffer's head
                        Array.Copy(Buffer, BufferOffset - remainCount, newBuffer, 0, remainCount);
                        Buffer = newBuffer;
                    }
                    else
                    {
                        Trace.WriteLine(
                                        $@"<<buffer recyling>>: available = {BufferCount}, need = {nextStepReqiredCount}, remained = {remainCount}");

                        // move remained data to buffer's head
                        Array.Copy(Buffer, BufferOffset - remainCount, Buffer, 0, remainCount);
                    }
                    BufferOffset = remainCount;
                    _decodeIndex = 0;
                }
                _previousRemainedCount = remainCount;
            }

            return _messageDataLength > 0;
        }

        private sealed class ItemListBuffer : IDisposable
        {
            private static readonly Pool<ItemListBuffer> Pool
                = new Pool<ItemListBuffer>(Create, poolAccessMode: PoolAccessMode.LIFO);

            private static ItemListBuffer Create(Pool<ItemListBuffer> p) => new ItemListBuffer(p);

            internal static ItemListBuffer Create(int capacity)
            {
                var result = Pool.Rent();
                result.Count = 0; // reset filled index
                result.PooledItems = new ArraySegment<SecsItem>(SecsItemArrayPool.Pool.Rent(capacity), 0, capacity);
                return result;
            }

            private ItemListBuffer(Pool<ItemListBuffer> pool)
            {
                _pool = pool;
            }

            private readonly Pool<ItemListBuffer> _pool;

            internal ArraySegment<SecsItem> PooledItems { get; private set; }

            internal void Add(SecsItem secsItem) => PooledItems.Array[Count++] = secsItem;

            internal int Capacity => PooledItems.Count;

            internal int Count { get; private set; }

            public void Dispose() => _pool.Return(this);
        }
    }
}
