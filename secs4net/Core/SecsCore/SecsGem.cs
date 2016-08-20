using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Secs4Net.Properties;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Secs4Net
{
    public sealed class SecsGem : IDisposable
    {
        public event EventHandler ConnectionChanged;
        public ConnectionState State { get; private set; }
        public short DeviceId { get; set; } = 0;
        public int LinkTestInterval { get; set; } = 60000;
        public int T3 { get; set; } = 45000;
        public int T5 { get; set; } = 10000;
        public int T6 { get; set; } = 5000;
        public int T7 { get; set; } = 10000;
        public int T8 { get; set; } = 5000;

        public bool LinkTestEnable
        {
            get { return _timerLinkTest.Enabled; }
            set
            {
                _timerLinkTest.Interval = LinkTestInterval;
                _timerLinkTest.Enabled = value;
            }
        }

        readonly bool _isActive;
        readonly IPAddress _ip;
        readonly int _port;
        Socket _socket;

        readonly SecsDecoder _secsDecoder;
        readonly ConcurrentDictionary<int, AsyncResult> _replyExpectedMsgs = new ConcurrentDictionary<int, AsyncResult>();
        readonly Action<SecsMessage, Action<SecsMessage>> _primaryMessageHandler;
        readonly SecsTracer _tracer;
        readonly System.Timers.Timer _timer7 = new System.Timers.Timer();	// between socket connected and received Select.req timer
        readonly System.Timers.Timer _timer8 = new System.Timers.Timer();
        readonly System.Timers.Timer _timerLinkTest = new System.Timers.Timer();

        readonly Func<Task> _startImpl;
        readonly Action _stopImpl;

        readonly byte[] _recvBuffer;
        static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
        static readonly ArraySegment<byte> ControlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });
        static readonly SecsTracer DefaultTracer = new SecsTracer();
        readonly Func<int> _newSystemByte;

        public SecsGem(IPAddress ip, int port, bool isActive, SecsTracer tracer = null, Action<SecsMessage, Action<SecsMessage>> primaryMsgHandler = null, int receiveBufferSize = 0x4000)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            _ip = ip;
            _port = port;
            _isActive = isActive;
            _recvBuffer = new byte[receiveBufferSize < 0x4000 ? 0x4000 : receiveBufferSize];
            _secsDecoder = new SecsDecoder(HandleControlMessage, HandleDataMessage);
            _tracer = tracer ?? DefaultTracer;
            _primaryMessageHandler = primaryMsgHandler ?? ((primary, reply) => reply(null));

            int systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
            _newSystemByte = () => Interlocked.Increment(ref systemByte);

            #region Timer Action
            _timer7.Elapsed += delegate
            {
                _tracer.TraceError("T7 Timeout");
                CommunicationStateChanging(ConnectionState.Retry);
            };

            _timer8.Elapsed += delegate
            {
                _tracer.TraceError("T8 Timeout");
                CommunicationStateChanging(ConnectionState.Retry);
            };

            _timerLinkTest.Elapsed += delegate
            {
                if (State == ConnectionState.Selected)
                    SendControlMessage(MessageType.LinkTestRequest, _newSystemByte());
            };
            #endregion
            if (_isActive)
            {
                #region Active Impl
                var timer5 = new System.Timers.Timer();
                timer5.Elapsed += delegate
                {
                    timer5.Enabled = false;
                    _tracer.TraceError("T5 Timeout");
                    CommunicationStateChanging(ConnectionState.Retry);
                };

                _startImpl = async delegate
                {
                    CommunicationStateChanging(ConnectionState.Connecting);
                    try
                    {
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        await Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, _ip, _port, null).ConfigureAwait(false);
                        CommunicationStateChanging(ConnectionState.Connected);
                        _socket = socket;
                        SendControlMessage(MessageType.SelectRequest, _newSystemByte());
                        _socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
                    }
                    catch (Exception ex)
                    {
                        if (_isDisposed) return;
                        _tracer.TraceError(ex.Message);
                        _tracer.TraceInfo("Start T5 Timer");
                        timer5.Interval = T5;
                        timer5.Enabled = true;
                    }
                };

                _stopImpl = delegate
                {
                    timer5.Stop();
                    if (_isDisposed) timer5.Dispose();
                };
                #endregion
            }
            else
            {
                #region Passive Impl
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(_ip, _port));
                server.Listen(0);

                _startImpl = async delegate
                {
                    CommunicationStateChanging(ConnectionState.Connecting);
                    try
                    {
                        _socket = await Task.Factory.FromAsync(server.BeginAccept, server.EndAccept, null).ConfigureAwait(false);
                        CommunicationStateChanging(ConnectionState.Connected);
                        _socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
                    }
                    catch (Exception ex)
                    {
                        _tracer.TraceError("System Exception", ex);
                        CommunicationStateChanging(ConnectionState.Retry);
                    }
                };

                _stopImpl = delegate
                {
                    if (_isDisposed)
                        server.Close();
                };
                #endregion
            }
        }

        void CommunicationStateChanging(ConnectionState newState)
        {
            State = newState;
            ConnectionChanged?.Invoke(this, EventArgs.Empty);

            switch (State)
            {
                case ConnectionState.Selected:
                    _timer7.Enabled = false;
                    _tracer.TraceInfo("Stop T7 Timer");
                    break;
                case ConnectionState.Connected:
                    _tracer.TraceInfo("Start T7 Timer");
                    _timer7.Interval = T7;
                    _timer7.Enabled = true;
                    break;
                case ConnectionState.Retry:
                    if (_isDisposed)
                        return;
                    Reset();
                    Thread.Sleep(2000);
                    _startImpl().Start();
                    break;
            }
        }

        void Reset()
        {
            _timer7.Stop();
            _timer8.Stop();
            _timerLinkTest.Stop();
            _secsDecoder.Reset();
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
            _replyExpectedMsgs.Clear();
            _stopImpl();
        }

        void ReceiveComplete(IAsyncResult iar)
        {
            try
            {
                int count = _socket.EndReceive(iar);

                _timer8.Enabled = false;

                if (count == 0)
                {
                    _tracer.TraceError("Received 0 byte data. Close the socket.");
                    CommunicationStateChanging(ConnectionState.Retry);
                    return;
                }

                if (_secsDecoder.Decode(_recvBuffer, 0, count))
                {
                    _tracer.TraceInfo("Start T8 Timer");
                    _timer8.Interval = T8;
                    _timer8.Enabled = true;
                }

                _socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
            }
            catch (NullReferenceException ex)
            {
                _tracer.TraceWarning("unexpected NullReferenceException:" + ex);
            }
            catch (SocketException ex)
            {
                _tracer.TraceError($"RecieveComplete socket error:{ex.Message + ex}, ErrorCode:{ex.SocketErrorCode}", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
            catch (Exception ex)
            {
                _tracer.TraceError("unexpected exception", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        void HandleControlMessage(Header header)
        {
            int systembyte = header.SystemBytes;
            if ((byte)header.MessageType % 2 == 0)
            {
                AsyncResult ar;
                if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
                {
                    ar.EndProcess(ControlMessage, false);
                }
                else
                {
                    _tracer.TraceWarning("Received Unexpected Control Message: " + header.MessageType);
                    return;
                }
            }
            _tracer.TraceInfo("Receive Control message: " + header.MessageType);
            switch (header.MessageType)
            {
                case MessageType.SelectRequest:
                    SendControlMessage(MessageType.SelectResponse, systembyte);
                    CommunicationStateChanging(ConnectionState.Selected);
                    break;
                case MessageType.SelectResponse:
                    switch (header.F)
                    {
                        case 0:
                            CommunicationStateChanging(ConnectionState.Selected);
                            break;
                        case 1:
                            _tracer.TraceError("Communication Already Active.");
                            break;
                        case 2:
                            _tracer.TraceError("Connection Not Ready.");
                            break;
                        case 3:
                            _tracer.TraceError("Connection Exhaust.");
                            break;
                        default:
                            _tracer.TraceError("Connection Status Is Unknown.");
                            break;
                    }
                    break;
                case MessageType.LinkTestRequest:
                    SendControlMessage(MessageType.LinkTestResponse, systembyte);
                    break;
                case MessageType.SeperateRequest:
                    CommunicationStateChanging(ConnectionState.Retry);
                    break;
            }
        }

        void HandleDataMessage(Header header, SecsMessage msg)
        {
            int systembyte = header.SystemBytes;

            if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
            {
                _tracer.TraceMessageIn(msg, systembyte);
                _tracer.TraceWarning("Received Unrecognized Device Id Message");
                try
                {
                    SendDataMessage(new SecsMessage(9, 1, false, "Unrecognized Device Id", Item.B(header.Bytes)), _newSystemByte());
                }
                catch (Exception ex)
                {
                    _tracer.TraceError("Send S9F1 Error", ex);
                }
                return;
            }

            if (msg.F % 2 != 0)
            {
                if (msg.S != 9)
                {
                    //Primary message
                    _tracer.TraceMessageIn(msg, systembyte);
                    _primaryMessageHandler(msg, secondary =>
                    {
                        if (!header.ReplyExpected || State != ConnectionState.Selected)
                            return;

                        secondary = secondary ?? new SecsMessage(9, 7, false, "Unknown Message", Item.B(header.Bytes));
                        secondary.ReplyExpected = false;
                        try
                        {
                            SendDataMessage(secondary, secondary.S == 9 ? _newSystemByte() : header.SystemBytes);
                        }
                        catch (Exception ex)
                        {
                            _tracer.TraceError("Reply Secondary Message Error", ex);
                        }
                    });
                    return;
                }
                // Error message
                var headerBytes = msg.SecsItem.GetValue<byte[]>();
                systembyte = BitConverter.ToInt32(new[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
            }

            // Secondary message
            _tracer.TraceMessageIn(msg, systembyte);
            AsyncResult ar;
            if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
                ar.EndProcess(msg, false);
        }

        void SendControlMessage(MessageType msgType, int systembyte)
        {
            if (_socket == null || !_socket.Connected)
                return;

            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                var ar = new AsyncResult(ControlMessage);
                _replyExpectedMsgs[systembyte] = ar;

                ThreadPool.RegisterWaitForSingleObject(
                    waitObject: ar.AsyncWaitHandle,
                    callBack: (state, timeout) =>
                    {
                        AsyncResult ars;
                        if (_replyExpectedMsgs.TryRemove((int)state, out ars) && timeout)
                        {
                            _tracer.TraceError("T6 Timeout");
                            CommunicationStateChanging(ConnectionState.Retry);
                        }
                    },
                    state: systembyte,
                    millisecondsTimeOutInterval:T6,
                    executeOnlyOnce: true);
            }

            var header = new Header(new byte[10])
            {
                MessageType = msgType,
                SystemBytes = systembyte
            };
            header.Bytes[0] = 0xFF;
            header.Bytes[1] = 0xFF;
            _socket.Send(new List<ArraySegment<byte>>(2){
                ControlMessageLengthBytes,
                new ArraySegment<byte>(header.Bytes)
            });
            _tracer.TraceInfo("Sent Control Message: " + header.MessageType);
        }

        AsyncResult SendDataMessage(SecsMessage msg, int systembyte)
        {
            if (State != ConnectionState.Selected)
                throw new SecsException("Device is not selected");

            var header = new Header(new byte[10])
            {
                S = msg.S,
                F = msg.F,
                ReplyExpected = msg.ReplyExpected,
                DeviceId = DeviceId,
                SystemBytes = systembyte
            };
            var buffer = new EncodedBuffer(header.Bytes, msg.RawDatas);

            AsyncResult ar = new AsyncResult(msg);
            if (msg.ReplyExpected)
            {
                _replyExpectedMsgs[systembyte] = ar;
                ThreadPool.RegisterWaitForSingleObject(
                   waitObject: ar.AsyncWaitHandle,
                   callBack: (state, timeout) =>
                   {
                       AsyncResult ars;
                       if (!_replyExpectedMsgs.TryRemove((int)state, out ars) || !timeout)
                           return;
                       _tracer.TraceError($"T3 Timeout[id=0x{state:X8}]");
                       ars.EndProcess(null, true);
                   }, 
                   state: systembyte,
                   millisecondsTimeOutInterval: T3,
                   executeOnlyOnce: true);
            }

            SocketError error;
            _socket.Send(buffer, SocketFlags.None, out error);
            if (error != SocketError.Success)
            {
                var errorMsg = "Socket send error :" + new SocketException((int)error).Message;
                _tracer.TraceError(errorMsg);
                CommunicationStateChanging(ConnectionState.Retry);
                throw new SecsException(errorMsg);
            }

            _tracer.TraceMessageOut(msg, systembyte);
            new Task(() => ar.AsyncWaitHandle.WaitOne()).Start();
            return ar;
        }

        public async Task Start() => await _startImpl();

        /// <summary>
        /// Send SECS message asynchronously to device.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public AsyncResult SendAsync(SecsMessage msg) => SendDataMessage(msg, _newSystemByte());

        volatile bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                ConnectionChanged = null;
                if (State == ConnectionState.Selected)
                    SendControlMessage(MessageType.SeperateRequest, _newSystemByte());
                Reset();
                _timer7.Dispose();
                _timer8.Dispose();
                _timerLinkTest.Dispose();
            }
        }

        public string DeviceAddress => _isActive
            ? _ip.ToString()
            : ((IPEndPoint)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        public struct AsyncResult : INotifyCompletion
        {
            internal ManualResetEvent AsyncWaitHandle { get; }
            readonly SecsMessage _primary;
            Action _onComplete;
            SecsMessage _secondary;
            bool _timeout;

            internal AsyncResult(SecsMessage primaryMsg) : this()
            {
                IsCompleted = !primaryMsg.ReplyExpected;
                AsyncWaitHandle = new ManualResetEvent(false);
                _primary = primaryMsg;
            }

            internal void EndProcess(SecsMessage replyMsg, bool timeout)
            {
                if (replyMsg != null)
                {
                    _secondary = replyMsg;
                    _secondary.Name = _primary.Name;
                }
                _timeout = timeout;
                IsCompleted = !timeout;
                AsyncWaitHandle.Set();
                _onComplete?.Invoke();
            }

            public void OnCompleted(Action continuation)
            {
                _onComplete = continuation;
            }

            public SecsMessage GetResult()
            {
                if (_timeout) throw new SecsException(_primary, Resources.T3Timeout);
                if (_secondary == null) return null;
                if (_secondary.F == 0) throw new SecsException(_primary, Resources.SxF0);
                if (_secondary.S == 9)
                {
                    switch (_secondary.F)
                    {
                        case 1: throw new SecsException(_primary, Resources.S9F1);
                        case 3: throw new SecsException(_primary, Resources.S9F3);
                        case 5: throw new SecsException(_primary, Resources.S9F5);
                        case 7: throw new SecsException(_primary, Resources.S9F7);
                        case 9: throw new SecsException(_primary, Resources.S9F9);
                        case 11: throw new SecsException(_primary, Resources.S9F11);
                        case 13: throw new SecsException(_primary, Resources.S9F13);
                        default: throw new SecsException(_primary, Resources.S9Fy);
                    }
                }
                return _secondary;
            }

            public AsyncResult GetAwaiter() => this;

            public bool IsCompleted { get; private set; }
        }

        #region SECS Decoder
        sealed class SecsDecoder
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
            Header _msgHeader; // message header
            readonly Stack<List<Item>> _stack = new Stack<List<Item>>(); // List Item stack
            SecsFormat _format;
            byte _lengthBits;
            int _itemLength;
            #endregion

            /// <summary>
            /// decode pipeline
            /// </summary>
            readonly Decoder[] _decoders;
            readonly Action<Header, SecsMessage> _dataMsgHandler;
            readonly Action<Header> _controlMsgHandler;

            internal SecsDecoder(Action<Header> controlMsgHandler, Action<Header, SecsMessage> msgHandler)
            {
                _dataMsgHandler = msgHandler;
                _controlMsgHandler = controlMsgHandler;

                _decoders = new Decoder[]{
                    #region _decoders[0]: get total message length 4 bytes
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                       if (!CheckAvailable(length, index, 4, out need)) return 0;

                       Array.Reverse(data, index, 4);
                       _messageLength = BitConverter.ToUInt32(data, index);
                       Trace.WriteLine("Get Message Length =" + _messageLength);
                       index += 4;

                       return 1;
                    },
                    #endregion
                    #region _decoders[1]: get message header 10 bytes
                    (byte[] data, int length, ref int index, out int need) =>
                    {
                        if (!CheckAvailable(length, index, 10, out need)) return 1;

                        _msgHeader = new Header(new byte[10]);
                        Array.Copy(data, index, _msgHeader.Bytes, 0, 10);
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

                            item = _itemLength == 0 ? _format.BytesDecode() : _format.BytesDecode(data, index, _itemLength);
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
                        Trace.WriteLine($"Remain Length: {_remainBytes.Offset}, Need:{_remainBytes.Count}");
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
                    Trace.WriteLine($"Remain Length: {_remainBytes.Offset}, Need:{_remainBytes.Count}");
                }
                return _messageLength > 0;
            }

            int _currentStep;
            /// <summary>
            /// 將位元組通過decode pipeline處理
            /// </summary>
            /// <param name="bytes">位元組</param>
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
        #endregion
        #region Message Header Struct
        struct Header
        {
            internal readonly byte[] Bytes;
            internal Header(byte[] headerbytes)
            {
                Bytes = headerbytes;
            }

            public short DeviceId
            {
                get
                {
                    return BitConverter.ToInt16(new[] { Bytes[1], Bytes[0] }, 0);
                }
                set
                {
                    byte[] values = BitConverter.GetBytes(value);
                    Bytes[0] = values[1];
                    Bytes[1] = values[0];
                }
            }
            public bool ReplyExpected
            {
                get { return (Bytes[2] & 0x80) == 0x80; }
                set { Bytes[2] = (byte)(S | (value ? 0x80 : 0)); }
            }
            public byte S
            {
                get { return (byte)(Bytes[2] & 0x7F); }
                set { Bytes[2] = (byte)(value | (ReplyExpected ? 0x80 : 0)); }
            }
            public byte F
            {
                get { return Bytes[3]; }
                set { Bytes[3] = value; }
            }
            public MessageType MessageType
            {
                get { return (MessageType)Bytes[5]; }
                set { Bytes[5] = (byte)value; }
            }
            public int SystemBytes
            {
                get
                {
                    return BitConverter.ToInt32(new[] {
                        Bytes[9],
                        Bytes[8],
                        Bytes[7],
                        Bytes[6]
                    }, 0);
                }
                set
                {
                    byte[] values = BitConverter.GetBytes(value);
                    Bytes[6] = values[3];
                    Bytes[7] = values[2];
                    Bytes[8] = values[1];
                    Bytes[9] = values[0];
                }
            }
        }
        #endregion
        #region EncodedByteList Wrapper just need IList<T>.Count and Indexer
        sealed class EncodedBuffer : IList<ArraySegment<byte>>
        {
            readonly IReadOnlyList<RawData> _data;// raw data include first message length 4 byte
            readonly byte[] _header;

            internal EncodedBuffer(byte[] header, IReadOnlyList<RawData> msgRawDatas)
            {
                _header = header;
                _data = msgRawDatas;
            }

            #region IList<ArraySegment<byte>> Members
            int IList<ArraySegment<byte>>.IndexOf(ArraySegment<byte> item) => -1;
            void IList<ArraySegment<byte>>.Insert(int index, ArraySegment<byte> item) { }
            void IList<ArraySegment<byte>>.RemoveAt(int index) { }
            ArraySegment<byte> IList<ArraySegment<byte>>.this[int index]
            {
                get { return new ArraySegment<byte>(index == 1 ? _header : _data[index].Bytes); }
                set { }
            }
            #endregion
            #region ICollection<ArraySegment<byte>> Members
            void ICollection<ArraySegment<byte>>.Add(ArraySegment<byte> item) { }
            void ICollection<ArraySegment<byte>>.Clear() { }
            bool ICollection<ArraySegment<byte>>.Contains(ArraySegment<byte> item) => false;
            void ICollection<ArraySegment<byte>>.CopyTo(ArraySegment<byte>[] array, int arrayIndex) { }
            int ICollection<ArraySegment<byte>>.Count => _data.Count;
            bool ICollection<ArraySegment<byte>>.IsReadOnly => true;
            bool ICollection<ArraySegment<byte>>.Remove(ArraySegment<byte> item) => false;
            #endregion
            #region IEnumerable<ArraySegment<byte>> Members
            public IEnumerator<ArraySegment<byte>> GetEnumerator()
            {
                for (int i = 0, length = _data.Count; i < length; i++)
                    yield return new ArraySegment<byte>(i == 1 ? _header : _data[i].Bytes);
            }
            #endregion
            #region IEnumerable Members
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            #endregion
        }
        #endregion
    }
}