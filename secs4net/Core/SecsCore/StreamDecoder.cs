using System;
using System.Collections.Generic;

namespace Secs4Net
{
    sealed class StreamDecoder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="need"></param>
        /// <returns>pipeline decoder index</returns>
        delegate int Decoder(byte[] data, int length, ref int index, out int need);
        #region share
        uint _messageLength;// total byte length
        MessageHeader _msgHeader; // message header
        readonly Stack<List<Item>> _stack = new Stack<List<Item>>(); // List Item stack
        SecsFormat _format;
        byte _lengthBits;
        int _itemLength;
        #endregion

        readonly ISecsGemLogger _logger;
        /// <summary>
        /// decode pipeline
        /// </summary>
        readonly Decoder[] _decoders;
        readonly Action<MessageHeader, SecsMessage> _dataMsgHandler;
        readonly Action<MessageHeader> _controlMsgHandler;

        internal StreamDecoder(ISecsGemLogger logger, Action<MessageHeader> controlMsgHandler, Action<MessageHeader, SecsMessage> dataMsgHandler)
        {
            _logger = logger;
            _dataMsgHandler = dataMsgHandler;
            _controlMsgHandler = controlMsgHandler;

            _decoders = new Decoder[]{
                    #region _decoders[0]: get total message length 4 bytes
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                       if (!CheckAvailable(length, index, 4, out need)) return 0;

                       Array.Reverse(data, index, 4);
                       _messageLength = BitConverter.ToUInt32(data, index);
                       _logger.TraceDebug("Get Message Length =" + _messageLength);
                       index += 4;

                       return 1;
                    },
                    #endregion
                    #region _decoders[1]: get message header 10 bytes
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                        if (!CheckAvailable(length, index, 10, out need)) return 1;

                        _msgHeader = new MessageHeader(data,index);
                        index += 10;
                        _messageLength -= 10;
                        if (_messageLength == 0)
                        {
                            if (_msgHeader.MessageType == MessageType.DataMessage)
                            {
                                ProcessMessage(new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty));
                            }
                            else
                            {
                                _controlMsgHandler(_msgHeader);
                                _messageLength = 0;
                            }
                            return 0;
                        }
                        else if (length - index >= _messageLength)
                        {
                            ProcessMessage(new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, data, ref index));
                            return 0; //completeWith message received
                        }
                        return 2;
                    },
                    #endregion
                    #region _decoders[2]: get _format + lengthBits(2bit) 1 byte
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                        if (!CheckAvailable(length, index, 1, out need)) return 2;

                        _format = (SecsFormat)(data[index] & 0xFC);
                        _lengthBits = (byte)(data[index] & 3);
                        index++;
                        _messageLength--;
                        return 3;
                    },
                    #endregion
                    #region _decoders[3]: get _itemLength _lengthBits bytes
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                        if (!CheckAvailable(length, index, _lengthBits, out need)) return 3;

                        byte[] itemLengthBytes = new byte[4];
                        Array.Copy(data, index, itemLengthBytes, 0, _lengthBits);
                        Array.Reverse(itemLengthBytes, 0, _lengthBits);

                        _itemLength = BitConverter.ToInt32(itemLengthBytes, 0);
                        Array.Clear(itemLengthBytes, 0, 4);

                        index += _lengthBits;
                        _messageLength -= _lengthBits;
                        return 4;
                    },
                    #endregion
                    #region _decoders[4]: get item value
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                        need = 0;
                        Item item;
                        if (_format == SecsFormat.List)
                        {
                            if (_itemLength == 0) {
                                item = Item.L();
                            }
                            else
                            {
                                _stack.Push(new List<Item>(_itemLength));
                                return 2;
                            }
                        }
                        else
                        {
                            if (!CheckAvailable(length, index, _itemLength, out need)) return 4;

                            item = _itemLength == 0 ? _format.BytesDecode() : _format.BytesDecode(new ArraySegment<byte>(data, index, _itemLength));
                            index += _itemLength;
                            _messageLength -= (uint)_itemLength;
                        }

                        if (_stack.Count > 0)
                        {
                            var list = _stack.Peek();
                            list.Add(item);
                            while (list.Count == list.Capacity)
                            {
                                item = Item.L(_stack.Pop());
                                if (_stack.Count > 0)
                                {
                                    list = _stack.Peek();
                                    list.Add(item);
                                }
                                else
                                {
                                    ProcessMessage(new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, string.Empty, item));
                                    return 0;
                                }
                            }
                        }
                        return 2;
                    },
                    #endregion
                };
        }

        void ProcessMessage(SecsMessage msg)
        {
            _dataMsgHandler(_msgHeader, msg);
            _messageLength = 0;
        }

        static bool CheckAvailable(int length, int index, int requireCount, out int need)
        {
            need = requireCount - (length - index);
            return need <= 0;
        }

        public void Reset()
        {
            _stack.Clear();
            _currentStep = 0;
            _remainBytes = new ArraySegment<byte>();
            _messageLength = 0;
        }

        /// <summary>
        /// Offset: next fill index
        /// Cout : next fill count
        /// </summary>
        ArraySegment<byte> _remainBytes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes">位元組</param>
        /// <param name="index">有效位元的起始索引</param>
        /// <param name="length">有效位元長度</param>
        /// <returns>如果輸入的位元組經解碼後尚有不足則回傳true,否則回傳false</returns>
        public bool Decode(byte[] bytes, int index, int length)
        {
            if (_remainBytes.Count == 0)
            {
                int need = Decode(bytes, length, ref index);
                int remainLength = length - index;
                if (remainLength > 0)
                {
                    var temp = new byte[remainLength + need];
                    Array.Copy(bytes, index, temp, 0, remainLength);
                    _remainBytes = new ArraySegment<byte>(temp, remainLength, need);
                    _logger.TraceDebug($"Remain Length: {_remainBytes.Offset}, Need:{_remainBytes.Count}");
                }
                else
                {
                    _remainBytes = new ArraySegment<byte>();
                }
            }
            else if (length - index >= _remainBytes.Count)
            {
                Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, _remainBytes.Count);
                index = _remainBytes.Count;
                byte[] temp = _remainBytes.Array;
                _remainBytes = new ArraySegment<byte>();
                if (Decode(temp, 0, temp.Length))
                    Decode(bytes, index, length);
            }
            else
            {
                int remainLength = length - index;
                Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, remainLength);
                _remainBytes = new ArraySegment<byte>(_remainBytes.Array, _remainBytes.Offset + remainLength, _remainBytes.Count - remainLength);
                _logger.TraceDebug($"Remain Length: {_remainBytes.Offset}, Need:{_remainBytes.Count}");
            }
            return _messageLength > 0;
        }

        int _currentStep;
        /// <summary>
        /// Use decode pipeline to handles message's bytes 
        /// </summary>
        /// <param name="bytes">data bytes</param>
        /// <param name="length">有效位元的起始索引</param>
        /// <param name="index">位元組的起始索引</param>
        /// <returns>回傳_currentStep不足的byte數</returns>
        int Decode(byte[] bytes, int length, ref int index)
        {
            int need;
            int nexStep = _currentStep;
            do
            {
                _currentStep = nexStep;
                nexStep = _decoders[_currentStep](bytes, length, ref index, out need);
            } while (nexStep != _currentStep);
            return need;
        }
    }
}
