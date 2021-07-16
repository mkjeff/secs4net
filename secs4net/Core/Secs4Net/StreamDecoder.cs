using Microsoft.Toolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Secs4Net
{
    /// <summary>
    ///  Stream based HSMS/SECS-II message decoder
    /// </summary>
    internal sealed class StreamDecoder
    {
        public byte[] Buffer => _buffer;

        /// <summary>
        /// Control the range of data receiver 
        /// </summary>
        public int BufferOffset => _bufferOffset;
        public int BufferCount => Buffer.Length - _bufferOffset;

        /// <summary>
        /// decoder step
        /// </summary>
        /// <param name="length"></param>
        /// <param name="need"></param>
        /// <returns>pipeline decoder index</returns>
        private delegate int Decoder(ref int length, out int need);

        /// <summary>
        /// decode pipelines
        /// </summary>
        private readonly Decoder[] _decoders;

        private int _decoderStep;

        private readonly Action<MessageHeader, SecsMessage> _dataMsgHandler;
        private readonly Action<MessageHeader> _controlMsgHandler;

        /// <summary>
        /// data buffer
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// Control the range of data decoder
        /// </summary>
        private int _decodeIndex;


        private int _bufferOffset;

        /// <summary>
        /// previous decoded remained count
        /// </summary>
        private int _previousRemainedCount;

        private readonly Stack<List<Item>> _stack = new();
        private uint _messageDataLength;
        private MessageHeader _msgHeader;
        private SecsFormat _format;
        private byte _lengthByteCount;
        private int _itemLength;

        public void Reset()
        {
            _stack.Clear();
            _decoderStep = 0;
            _decodeIndex = 0;
            _bufferOffset = 0;
            _messageDataLength = 0;
            _previousRemainedCount = 0;
        }

        internal StreamDecoder(int initialBufferSize, Action<MessageHeader> controlMsgHandler, Action<MessageHeader, SecsMessage> dataMsgHandler)
        {
            _buffer = new byte[initialBufferSize];
            _bufferOffset = 0;
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

            // 0: get total message length 4 bytes
            int GetTotalMessageLength(ref int length, out int need)
            {
                if (!CheckAvailable(length, 4, out need))
                {
                    return 0;
                }

                Array.Reverse(_buffer, _decodeIndex, 4);
                _messageDataLength = BitConverter.ToUInt32(_buffer, _decodeIndex);
                Trace.WriteLine($"Get Message Length: {_messageDataLength}");
                _decodeIndex += 4;
                length -= 4;
                return GetMessageHeader(ref length, out need);
            }

            // 1: get message header 10 bytes
            int GetMessageHeader(ref int length, out int need)
            {
                if (!CheckAvailable(length, 10, out need))
                {
                    return 1;
                }

                _msgHeader = MessageHeader.Decode(_buffer.AsSpan(_decodeIndex, 10));
                _decodeIndex += 10;
                _messageDataLength -= 10;
                length -= 10;
                if (_messageDataLength == 0)
                {
                    if (_msgHeader.MessageType == MessageType.DataMessage)
                    {
                        _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, replyExpected: _msgHeader.ReplyExpected));
                    }
                    else
                    {
                        _controlMsgHandler(_msgHeader);
                    }

                    return 0;
                }

                if (length >= _messageDataLength)
                {
                    Trace.WriteLine("Get Complete Data Message with total data");
                    _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected)
                    {
                        SecsItem = Item.DecodeFromFullBuffer(_buffer, ref _decodeIndex),
                    });
                    length -= (int)_messageDataLength;
                    _messageDataLength = 0;
                    return 0; //completeWith message received
                }
                return GetItemHeader(ref length, out need);
            }

            // 2: get _format + _lengthByteCount(2bit) 1 byte
            int GetItemHeader(ref int length, out int need)
            {
                if (!CheckAvailable(length, 1, out need))
                {
                    return 2;
                }

                Item.DecodeFormatAndLengthByteCount(_buffer.AsSpan(), ref _decodeIndex, out _format, out _lengthByteCount);
                _messageDataLength--;
                length--;
                return GetItemLength(ref length, out need);
            }

            // 3: get _itemLength bytes(size= _lengthByteCount), at most 3 byte
            int GetItemLength(ref int length, out int need)
            {
                if (!CheckAvailable(length, _lengthByteCount, out need))
                {
                    return 3;
                }

                Item.DecodeDataLength(_buffer.AsSpan(_decodeIndex, _lengthByteCount), ref _decodeIndex, out _itemLength);
                Trace.WriteLine($"Get format: {_format}, length: {_itemLength}");

                _messageDataLength -= _lengthByteCount;
                length -= _lengthByteCount;
                return GetItem(ref length, out need);
            }

            // 4: get item value
            int GetItem(ref int length, out int need)
            {
                need = 0;
                Item item;
                if (_format == SecsFormat.List)
                {
                    if (_itemLength == 0)
                    {
                        item = Item.L();
                    }
                    else
                    {
                        _stack.Push(new List<Item>(_itemLength));
                        return GetItemHeader(ref length, out need);
                    }
                }
                else
                {
                    if (!CheckAvailable(length, _itemLength, out need))
                    {
                        return 4;
                    }

                    item = Item.DecodeNonListItem(_format, _buffer.AsSpan(_decodeIndex, _itemLength));
                    Trace.WriteLine($"Complete Item: {_format}");

                    _decodeIndex += _itemLength;
                    _messageDataLength -= (uint)_itemLength;
                    length -= _itemLength;
                }

                if (_stack.Count == 0)
                {
                    Trace.WriteLine("Get Complete Data Message by stream decoded");
                    _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected)
                    {
                        SecsItem = item,
                    });
                    return 0;
                }

                var list = _stack.Peek();
                list.Add(item);
                while (list.Count == list.Capacity)
                {
                    item = Item.L(_stack.Pop());
                    Trace.WriteLine($"Complete List: {item.Count}");
                    if (_stack.Count > 0)
                    {
                        list = _stack.Peek();
                        list.Add(item);
                    }
                    else
                    {
                        Trace.WriteLine("Get Complete Data Message by stream decoded");
                        _dataMsgHandler(_msgHeader, new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected)
                        {
                            SecsItem = item,
                        });
                        return 0;
                    }
                }

                return GetItemHeader(ref length, out need);
            }

            static bool CheckAvailable(int length, int required, out int need)
            {
                need = required - length;
                if (need > 0)
                {
                    return false;
                }
                need = 0;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">data length</param>
        /// <returns>true, if need more data to decode completed message. otherwise, return false</returns>
        public bool Decode(int length)
        {
            Debug.Assert(length > 0, "decode data length is 0.");
            var decodeLength = length;
            length += _previousRemainedCount; // total available length = current length + previous remained
            int need;
            var nexStep = _decoderStep;
            do
            {
                _decoderStep = nexStep;
                nexStep = _decoders[_decoderStep](ref length, out need);
            } while (nexStep != _decoderStep);

            Debug.Assert(_decodeIndex >= _bufferOffset, "decode index should ahead of buffer index");

            var remainCount = length;
            Debug.Assert(remainCount >= 0, "remain count is only possible grater and equal zero");
            Trace.WriteLine($"remain data length: {remainCount}");
            Trace.WriteLineIf(_messageDataLength > 0, $"need data count: {need}");

            if (remainCount == 0)
            {
                if (need > Buffer.Length)
                {
                    var newSize = need * 2;
                    Trace.WriteLine($@"<<buffer resizing>>: current size = {_buffer.Length}, new size = {newSize}");

                    // increase buffer size
                    _buffer = new byte[newSize];
                }
                _bufferOffset = 0;
                _decodeIndex = 0;
                _previousRemainedCount = 0;
            }
            else
            {
                _bufferOffset += decodeLength; // move next receive index
                var nextStepReqiredCount = remainCount + need;
                if (nextStepReqiredCount > BufferCount)
                {
                    if (nextStepReqiredCount > Buffer.Length)
                    {
                        var newSize = Math.Max(_messageDataLength / 2, nextStepReqiredCount) * 2;
                        Trace.WriteLine($@"<<buffer resizing>>: current size = {_buffer.Length}, remained = {remainCount}, new size = {newSize}");

                        // out of total buffer size
                        // increase buffer size
                        var newBuffer = new byte[newSize];
                        // keep remained data to new buffer's head
                        Array.Copy(_buffer, _bufferOffset - remainCount, newBuffer, 0, remainCount);
                        _buffer = newBuffer;
                    }
                    else
                    {
                        Trace.WriteLine($@"<<buffer recyling>>: available = {BufferCount}, need = {nextStepReqiredCount}, remained = {remainCount}");

                        // move remained data to buffer's head
                        Array.Copy(_buffer, _bufferOffset - remainCount, _buffer, 0, remainCount);
                    }
                    _bufferOffset = remainCount;
                    _decodeIndex = 0;
                }
                _previousRemainedCount = remainCount;
            }

            return _messageDataLength > 0;
        }
    }
}
