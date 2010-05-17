using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Secs4Net {
    public sealed class SecsGem : IDisposable {
        public event EventHandler ConnectionChanged;
        public ConnectionState State { get; private set; }
        public short DeviceId { get; set; }
        public int LinkTestInterval { get; set; }
        public int T3 { get; set; }
        public int T5 { get; set; }
        public int T6 { get; set; }
        public int T7 { get; set; }
        public int T8 { get; set; }

        public bool LinkTestEnable {
            get { return _timerLinkTest.Enabled; }
            set {
                _timerLinkTest.Interval = LinkTestInterval;
                _timerLinkTest.Enabled = value;
            }
        }

        bool _isActive;
        readonly IPAddress _ip;
        readonly int _port;
        Socket _socket;

        readonly SecsDecoder _secsDecoder;
        readonly Dictionary<int, SecsAsyncResult> _replyExpectedMsgs = new Dictionary<int, SecsAsyncResult>();
        readonly Action<SecsMessage, Action<SecsMessage>> PrimaryMessageHandler;
        readonly ISecsTracer _tracer;
        readonly System.Timers.Timer _timer7 = new System.Timers.Timer();	// between socket connected and recived Select.req timer
        readonly System.Timers.Timer _timer8 = new System.Timers.Timer();
        readonly System.Timers.Timer _timerLinkTest = new System.Timers.Timer();

        readonly Action StartImpl;
        readonly Action StopImpl;

        readonly byte[] _recvBuffer;
        static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
        static readonly byte[] ControlMessageLengthBytes = new byte[] { 0, 0, 0, 10 };
        static readonly ISecsTracer DefaultTracer = new DefaultSecsTracer();
        int _SystemBytes = new Random(int.MaxValue).Next();
        int NewSystemBytes() { return Interlocked.Increment(ref _SystemBytes); }

        public SecsGem(IPAddress ip, int port, bool isActive, int receiveBufferSize, ISecsTracer tracer, Action<SecsMessage, Action<SecsMessage>> primaryMsgHandler) {
            ip.CheckNull("ip");
            primaryMsgHandler.CheckNull("primaryMsgHandler");

            _ip = ip;
            _port = port;
            _isActive = isActive;
            _recvBuffer = new byte[receiveBufferSize];
            _secsDecoder = new SecsDecoder(HandleControlMessage, HandleDataMessage);
            _tracer = tracer ?? DefaultTracer;
            PrimaryMessageHandler = primaryMsgHandler;
            T3 = 45000;
            T5 = 10000;
            T6 = 5000;
            T7 = 10000;
            T8 = 5000;
            LinkTestInterval = 60000;

            #region Timer Action
            _timer7.Elapsed += delegate {
                _tracer.TraceError("T7 Timeout");
                this.CommunicationStateChanging(ConnectionState.Retry);
            };

            _timer8.Elapsed += delegate {
                _tracer.TraceError("T8 Timeout");
                this.CommunicationStateChanging(ConnectionState.Retry);
            };

            _timerLinkTest.Elapsed += delegate {
                if (this.State == ConnectionState.Selected)
                    this.SendControlMessage(MessageType.Linktest_req, NewSystemBytes());
            };
            #endregion
            if (_isActive) {
                #region Active Impl
                var _timer5 = new System.Timers.Timer();
                _timer5.Elapsed += delegate {
                    _timer5.Enabled = false;
                    _tracer.TraceError("T5 Timeout");
                    this.CommunicationStateChanging(ConnectionState.Retry);
                };

                StartImpl = delegate {
                    this.CommunicationStateChanging(ConnectionState.Connecting);
                    try {
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(_ip, _port);
                        this.CommunicationStateChanging(ConnectionState.Connected);
                        this._socket = socket;
                        this.SendControlMessage(MessageType.Select_req, NewSystemBytes());
                        this._socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
                    } catch (Exception ex) {
                        if (_isDisposed) return;
                        _tracer.TraceError(ex.Message);
                        _tracer.TraceInfo("Start T5 Timer");
                        _timer5.Interval = T5;
                        _timer5.Enabled = true;
                    }
                };

                StopImpl = delegate {
                    _timer5.Stop();
                };
                #endregion
                StartImpl.BeginInvoke(null, null);
            } else {
                #region Passive Impl
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(_ip, _port));
                server.Listen(0);

                StartImpl = delegate {
                    this.CommunicationStateChanging(ConnectionState.Connecting);
                    server.BeginAccept(iar => {
                        try {
                            this._socket = server.EndAccept(iar);
                            this.CommunicationStateChanging(ConnectionState.Connected);
                            this._socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
                        } catch (Exception ex) {
                            _tracer.TraceError("System Exception:" + ex.Message);
                            this.CommunicationStateChanging(ConnectionState.Retry);
                        }
                    }, null);
                };

                StopImpl = delegate {
                    if (_isDisposed && server != null)
                        server.Close();
                };
                #endregion
                StartImpl();
            }
        }

        #region Socket Receive Process
        void ReceiveComplete(IAsyncResult iar) {
            try {
                int count = _socket.EndReceive(iar);

                _timer8.Enabled = false;

                if (count == 0) {
                    _tracer.TraceError("Received 0 byte data. Close the socket.");
                    this.CommunicationStateChanging(ConnectionState.Retry);
                    return;
                }

                if (_secsDecoder.Decode(_recvBuffer, 0, count)) {
                    _tracer.TraceInfo("Start T8 Timer");
                    _timer8.Interval = T8;
                    _timer8.Enabled = true;
                }

                _socket.BeginReceive(_recvBuffer, 0, _recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
            } catch (NullReferenceException) {
            } catch (SocketException ex) {
                _tracer.TraceError("RecieveComplete socket error:" + ex.Message);
                this.CommunicationStateChanging(ConnectionState.Retry);
            } catch (Exception ex) {
                _tracer.TraceError(ex.ToString());
                this.CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        void HandleControlMessage(Header header) {
            int systembyte = header.SystemBytes;
            if ((byte)header.MessageType % 2 == 0) {
                SecsAsyncResult ar = null;
                if (_replyExpectedMsgs.TryGetValue(systembyte, out ar)) {
                    ar.EndProcess(ControlMessage, false);
                } else {
                    _tracer.TraceWarning("Received Unexpected Control Message: " + header.MessageType.ToString());
                    return;
                }
            }
            _tracer.TraceInfo("Receive Control message: " + header.MessageType.ToString());
            switch (header.MessageType) {
                case MessageType.Select_req:
                    this.SendControlMessage(MessageType.Select_rsp, systembyte);
                    this.CommunicationStateChanging(ConnectionState.Selected);
                    break;
                case MessageType.Select_rsp:
                    switch (header.F) {
                        case 0:
                            this.CommunicationStateChanging(ConnectionState.Selected);
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
                case MessageType.Linktest_req:
                    this.SendControlMessage(MessageType.Linktest_rsp, systembyte);
                    break;
                case MessageType.Seperate_req:
                    this.CommunicationStateChanging(ConnectionState.Retry);
                    break;
            }
        }

        void HandleDataMessage(Header header, SecsMessage msg) {
            int systembyte = header.SystemBytes;

            if (header.DeviceId != this.DeviceId && msg.S != 9 && msg.F != 1) {
                _tracer.TraceMessageIn(msg, systembyte);
                _tracer.TraceWarning("Received Unrecognized Device Id Message");
                try {
                    this.SendDataMessage(new SecsMessage(9, 1, "UnrecognizedDeviceId", false, Item.B(header.Bytes)), NewSystemBytes(), null, null);
                } catch (Exception ex) {
                    _tracer.TraceError("Send S9F1 Error:" + ex.Message);
                }
                return;
            }

            if (msg.F % 2 != 0) {
                if (msg.S != 9) {
                    //Primary message
                    _tracer.TraceMessageIn(msg, systembyte);
                    PrimaryMessageHandler(msg, secondary => {
                        if (header.ReplyExpected && State == ConnectionState.Selected) {
                            secondary = secondary ?? new SecsMessage(9, 7, "Unknow Message", false, Item.B(header.Bytes));
                            secondary.ReplyExpected = false;
                            try {
                                this.SendDataMessage(secondary, secondary.S == 9 ? NewSystemBytes() : header.SystemBytes, null, null);
                            } catch (Exception ex) {
                                _tracer.TraceError("Reply Secondary Message Error:" + ex.Message);
                            }
                        }
                    });
                    return;
                }
                // Error message
                var headerBytes = (byte[])msg.SecsItem;
                systembyte = BitConverter.ToInt32(new byte[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
            }

            // Secondary message
            SecsAsyncResult ar = null;
            if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
                ar.EndProcess(msg, false);
            _tracer.TraceMessageIn(msg, systembyte);
        }
        #endregion
        #region Socket Send Process
        void SendControlMessage(MessageType msgType, int systembyte) {
            if (this._socket == null || !this._socket.Connected)
                return;

            if ((byte)msgType % 2 == 1 && msgType != MessageType.Seperate_req) {
                var ar = new SecsAsyncResult(ControlMessage, 0, null, null);
                lock (((ICollection)_replyExpectedMsgs).SyncRoot)
                    _replyExpectedMsgs[systembyte] = ar;

                ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle,
                    (state, timeout) => {
                        lock (((ICollection)_replyExpectedMsgs).SyncRoot)
                            if (_replyExpectedMsgs.Remove((int)state) && timeout) {
                                _tracer.TraceError("T6 Timeout");
                                this.CommunicationStateChanging(ConnectionState.Retry);
                            }
                    }, systembyte, T6, true);
            }

            var header = new Header(new byte[10]) {
                MessageType = msgType,
                SystemBytes = systembyte
            };
            header.Bytes[0] = 0xFF;
            header.Bytes[1] = 0xFF;
            _socket.Send(new List<ArraySegment<byte>>(2){
                new ArraySegment<byte>(ControlMessageLengthBytes),
                new ArraySegment<byte>(header.Bytes)
            });
            _tracer.TraceInfo("Sent Control Message: " + header.MessageType);
        }

        SecsAsyncResult SendDataMessage(SecsMessage msg, int systembyte, AsyncCallback callback, object syncState) {
            if (this.State != ConnectionState.Selected)
                throw new SecsException("Device is not selected");

            bool replyExpected = msg.ReplyExpected;
            var header = new Header(new byte[10]) {
                S = msg.S,
                F = msg.F,
                ReplyExpected = replyExpected,
                DeviceId = this.DeviceId,
                SystemBytes = systembyte
            };
            var buffer = new EncodedBuffer(header.Bytes, msg.RawDatas);

            SecsAsyncResult ar = null;
            if (replyExpected) {
                ar = new SecsAsyncResult(msg, systembyte, callback, syncState);
                lock (((ICollection)_replyExpectedMsgs).SyncRoot)
                    _replyExpectedMsgs[systembyte] = ar;

                ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle,
                   (state, timeout) => {
                       var ars = (SecsAsyncResult)state;
                       lock (((ICollection)_replyExpectedMsgs).SyncRoot)
                           _replyExpectedMsgs.Remove(ars.SystemByte);
                       if (timeout) {
                           _tracer.TraceError("T3 Timeout[id=0x" + ars.SystemByte.ToString("X8") + "]");
                           ars.EndProcess(null, timeout);
                       }
                   }, ar, T3, true);
            }

            SocketError error;
            _socket.Send(buffer, SocketFlags.None, out error);
            if (error != SocketError.Success) {
                var errorMsg = "Socket send error :" + new SocketException((int)error).Message;
                _tracer.TraceError(errorMsg);
                this.CommunicationStateChanging(ConnectionState.Retry);
                throw new SecsException(errorMsg);
            }

            _tracer.TraceMessageOut(msg, systembyte);
            return ar;
        }
        #endregion
        #region Internal State Transition
        void CommunicationStateChanging(ConnectionState newState) {
            State = newState;
            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);

            switch (State) {
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
                    StartImpl();
                    break;
            }
        }

        void Reset() {
            this._timer7.Stop();
            this._timer8.Stop();
            this._timerLinkTest.Stop();
            this._secsDecoder.Reset();
            if (_socket != null) {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
            lock (((ICollection)_replyExpectedMsgs).SyncRoot)
                this._replyExpectedMsgs.Clear();
            StopImpl();
        }
        #endregion
        #region Public API
        /// <summary>
        /// Send SECS message to device.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Device's reply msg if msg.ReplyExpected is true;otherwise, null.</returns>
        public SecsMessage Send(SecsMessage msg) {
            return this.EndSend(this.BeginSend(msg, null, null));
        }

        /// <summary>
        /// Send SECS message asynchronously to device .
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="callback">Device's reply message handler callback.</param>
        /// <param name="state">synchronize state object</param>
        /// <returns>An IAsyncResult that references the asynchronous send if msg.ReplyExpected is true;otherwise, null.</returns>
        public IAsyncResult BeginSend(SecsMessage msg, AsyncCallback callback, object state) {
            return this.SendDataMessage(msg, NewSystemBytes(), callback, state);
        }

        /// <summary>
        /// Ends a asynchronous send.
        /// </summary>
        /// <param name="iar">An IAsyncResult that references the asynchronous send</param>
        /// <returns>Device's reply msg if iar is an IAsyncResult that references the asynchronous send;otherwise, null.</returns>
        public SecsMessage EndSend(IAsyncResult iar) {
            if (iar == null)
                return null;
            var ar = iar as SecsAsyncResult;
            if (ar == null)
                throw new ArgumentException("參數IAsyncResult不是BeginSend所取得的", "iar");
            ar.AsyncWaitHandle.WaitOne();
            return ar.Secondary;
        }

        volatile bool _isDisposed;
        public void Dispose() {
            if (!_isDisposed) {
                _isDisposed = true;
                ConnectionChanged = null;
                if (State == ConnectionState.Selected)
                    this.SendControlMessage(MessageType.Seperate_req, NewSystemBytes());
                Reset();
            }
        }

        public string DeviceAddress {
            get {
                return _isActive ? _ip.ToString() :
                    this._socket == null ? "N/A" : ((IPEndPoint)_socket.RemoteEndPoint).Address.ToString();
            }
        }
        #endregion
        #region Async Impl
        sealed class SecsAsyncResult : IAsyncResult {
            readonly ManualResetEvent _ev = new ManualResetEvent(false);
            readonly SecsMessage Primary;
            readonly AsyncCallback _callback;
            public readonly int SystemByte;

            SecsMessage _secondary;
            bool _timeout;

            internal SecsAsyncResult(SecsMessage primaryMsg, int systembyte, AsyncCallback callback, object state) {
                Primary = primaryMsg;
                AsyncState = state;
                SystemByte = systembyte;
                _callback = callback;
            }

            internal void EndProcess(SecsMessage replyMsg, bool timeout) {
                if (replyMsg != null) {
                    _secondary = replyMsg;
                    _secondary.Name = Primary.Name;
                    _secondary.Tag = Primary.Tag;
                }
                _timeout = timeout;
                IsCompleted = !timeout;
                _ev.Set();
                if (_callback != null)
                    _callback(this);
            }

            internal SecsMessage Secondary {
                get {
                    if (_timeout) throw new SecsException(Primary, "T3 Timeout!");
                    if (_secondary == null) return null;
                    if (_secondary.F == 0) throw new SecsException(Primary, "Equipment is not online mode");
                    if (_secondary.S == 9) {
                        switch (_secondary.F) {
                            case 1: throw new SecsException(Primary, "Unrecognized Device Id");
                            case 3: throw new SecsException(Primary, "Unrecognized Stream Type");
                            case 5: throw new SecsException(Primary, "Unrecognized Function Type");
                            case 7: throw new SecsException(Primary, "Illegal Data");
                            case 9: throw new SecsException(Primary, "Transaction Timer Timeout");
                            case 11: throw new SecsException(Primary, "Data Too Long");
                            case 13: throw new SecsException(Primary, "Conversation Timeout");
                            default: throw new SecsException(Primary, "S9Fy message reply.");
                        }
                    }
                    return _secondary;
                }
            }

            #region IAsyncResult Members

            public object AsyncState { get; private set; }

            public System.Threading.WaitHandle AsyncWaitHandle { get { return _ev; } }

            public bool CompletedSynchronously { get { return false; } }

            public bool IsCompleted { get; private set; }

            #endregion
        }
        #endregion
        #region SECS Decoder
        sealed class SecsDecoder {
            delegate int Decoder(byte[] data, int length, ref int index, out int need);
            #region share
            uint _messageLength;// total byte length
            Header _msgHeader; // message header
            SecsMessage _msg;
            readonly Stack<List<Item>> _stack = new Stack<List<Item>>(); // List Item stack
            SecsFormat _format;
            byte _lengthBits;
            readonly byte[] _itemLengthBytes = new byte[4];
            int _itemLength;
            #endregion

            /// <summary>
            /// decode pipeline
            /// </summary>
            readonly Decoder[] decoders;
            readonly Action<Header, SecsMessage> DataMsgHandler;
            readonly Action<Header> ControlMsgHandler;

            internal SecsDecoder(Action<Header> controlMsgHandler, Action<Header, SecsMessage> msgHandler) {
                DataMsgHandler = msgHandler;
                ControlMsgHandler = controlMsgHandler;

                decoders = new Decoder[5];

                #region decoders[0]: get total message length
                decoders[0] = (byte[] data, int length, ref int index, out int need) => {
                    if (!CheckAvailable(length, index, 4, out need)) return 0;

                    Array.Reverse(data, index, 4);
                    _messageLength = BitConverter.ToUInt32(data, index);
                    Trace.WriteLine("Get Message Length=" + _messageLength);
                    index += 4;

                    return 1;
                };
                #endregion
                #region decoders[1]: get message header 10 bytes
                decoders[1] = (byte[] data, int length, ref int index, out int need) => {
                    if (!CheckAvailable(length, index, 10, out need)) return 1;

                    _msgHeader = new Header(new byte[10]);
                    Array.Copy(data, index, _msgHeader.Bytes, 0, 10);
                    index += 10;
                    _messageLength -= 10;
                    if (_messageLength == 0) {
                        if (_msgHeader.MessageType == MessageType.Data_Message) {
                            _msg = new SecsMessage(_msgHeader.S, _msgHeader.F, string.Empty, _msgHeader.ReplyExpected, null);
                            ProcessMessage();
                        } else {
                            ControlMsgHandler(_msgHeader);
                            _messageLength = 0;
                        }
                        return 0;
                    } else if (length - index >= _messageLength) {
                        _msg = new SecsMessage(_msgHeader.S, _msgHeader.F, _msgHeader.ReplyExpected, data, ref index);
                        ProcessMessage();
                        return 0; //completeWith message received
                    }
                    return 2;
                };
                #endregion
                #region decoders[2]: get format + lengthBits(2bit) 1 byte
                decoders[2] = (byte[] data, int length, ref int index, out int need) => {
                    if (!CheckAvailable(length, index, 1, out need)) return 2;

                    _format = (SecsFormat)(data[index] & 0xFC);
                    _lengthBits = (byte)(data[index] & 3);
                    index++;
                    _messageLength--;
                    return 3;
                };
                #endregion
                #region decoders[3]: get length _lengthBits bytes
                decoders[3] = (byte[] data, int length, ref int index, out int need) => {
                    if (!CheckAvailable(length, index, _lengthBits, out need)) return 3;

                    Array.Copy(data, index, _itemLengthBytes, 0, _lengthBits);
                    Array.Reverse(_itemLengthBytes, 0, _lengthBits);

                    _itemLength = BitConverter.ToInt32(_itemLengthBytes, 0);
                    Array.Clear(_itemLengthBytes, 0, 4);

                    index += _lengthBits;
                    _messageLength -= _lengthBits;
                    return 4;
                };
                #endregion
                #region decoders[4]: get item value
                decoders[4] = (byte[] data, int length, ref int index, out int need) => {
                    need = 0;
                    Item item = null;
                    if (_format == SecsFormat.List) {
                        if (_itemLength == 0)
                            item = Item.L();
                        else {
                            _stack.Push(new List<Item>(_itemLength));
                            return 2;
                        }
                    } else {
                        if (!CheckAvailable(length, index, _itemLength, out need)) return 4;

                        item = SecsExtension.BytesDecoders[_format](data, index, length);
                        index += _itemLength;
                        _messageLength -= (uint)_itemLength;
                    }
                    if (_stack.Count > 0) {
                        var list = _stack.Peek();
                        list.Add(item);
                        while (list.Count == list.Capacity) {
                            item = Item.L(_stack.Pop());
                            if (_stack.Count > 0) {
                                list = _stack.Peek();
                                list.Add(item);
                            } else {
                                _msg = new SecsMessage(_msgHeader.S, _msgHeader.F, "Unknown", _msgHeader.ReplyExpected, item);
                                ProcessMessage();
                                return 0;
                            }
                        }
                    }
                    return 2;
                };
                #endregion
            }

            void ProcessMessage() {
                DataMsgHandler(_msgHeader, _msg);
                _msg = null;
                _messageLength = 0;
            }

            static bool CheckAvailable(int length, int index, int requireCount, out int need) {
                need = requireCount - (length - index);
                return need <= 0;
            }

            public void Reset() {
                _msg = null;
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
            public bool Decode(byte[] bytes, int index, int length) {
                if (_remainBytes.Count == 0) {
                    int need = Decode(bytes, length, ref index);
                    int remainLength = length - index;
                    if (remainLength > 0) {
                        var temp = new byte[remainLength + need];
                        Array.Copy(bytes, index, temp, 0, remainLength);
                        _remainBytes = new ArraySegment<byte>(temp, remainLength, need);
                        Trace.WriteLine("Remain Lenght: " + _remainBytes.Offset + ", Need:" + _remainBytes.Count);
                    } else {
                        _remainBytes = new ArraySegment<byte>();
                    }
                } else if (length - index >= _remainBytes.Count) {
                    Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, _remainBytes.Count);
                    index = _remainBytes.Count;
                    byte[] temp = _remainBytes.Array;
                    _remainBytes = new ArraySegment<byte>();
                    if (Decode(temp, 0, temp.Length))
                        Decode(bytes, index, length);
                } else {
                    int remainLength = length - index;
                    Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, remainLength);
                    _remainBytes = new ArraySegment<byte>(_remainBytes.Array, _remainBytes.Offset + remainLength, _remainBytes.Count - remainLength);
                    Trace.WriteLine("Remain Lenght: " + _remainBytes.Offset + ", Need:" + _remainBytes.Count);
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
            int Decode(byte[] bytes, int length, ref int index) {
                int nexStep = _currentStep;
                int need;
                do {
                    _currentStep = nexStep;
                    nexStep = decoders[_currentStep](bytes, length, ref index, out need);
                } while (nexStep != _currentStep);
                return need;
            }
        }
        #endregion
        #region Message Header Struct
        struct Header {
            internal readonly byte[] Bytes;
            internal Header(byte[] headerbytes) {
                Bytes = headerbytes;
            }

            public short DeviceId {
                get {
                    return BitConverter.ToInt16(new[] { Bytes[1], Bytes[0] }, 0);
                }
                set {
                    byte[] values = BitConverter.GetBytes(value);
                    Bytes[0] = values[1];
                    Bytes[1] = values[0];
                }
            }
            public bool ReplyExpected {
                get { return (Bytes[2] & 0x80) == 0x80; }
                set { Bytes[2] = (byte)(S | (value ? 0x80 : 0)); }
            }
            public byte S {
                get { return (byte)(Bytes[2] & 0x7F); }
                set { Bytes[2] = (byte)(value | (ReplyExpected ? 0x80 : 0)); }
            }
            public byte F {
                get { return Bytes[3]; }
                set { Bytes[3] = value; }
            }
            public MessageType MessageType {
                get { return (MessageType)Bytes[5]; }
                set { Bytes[5] = (byte)value; }
            }
            public int SystemBytes {
                get {
                    return BitConverter.ToInt32(new[] { 
                        Bytes[9], 
                        Bytes[8], 
                        Bytes[7], 
                        Bytes[6] 
                    }, 0);
                }
                set {
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
        sealed class EncodedBuffer : IList<ArraySegment<byte>> {
            readonly IList<RawData> _data;// raw data include first message length 4 byte
            readonly ArraySegment<byte> _header;

            internal EncodedBuffer(byte[] header, IList<RawData> msgRawDatas) {
                _header = new ArraySegment<byte>(header);
                _data = msgRawDatas;
            }

            #region IList<ArraySegment<byte>> Members
            int IList<ArraySegment<byte>>.IndexOf(ArraySegment<byte> item) { return -1; }
            void IList<ArraySegment<byte>>.Insert(int index, ArraySegment<byte> item) { }
            void IList<ArraySegment<byte>>.RemoveAt(int index) { }
            ArraySegment<byte> IList<ArraySegment<byte>>.this[int index] {
                get { return (index == 1) ? _header : new ArraySegment<byte>(_data[index].Bytes); }
                set { }
            }
            #endregion
            #region ICollection<ArraySegment<byte>> Members
            void ICollection<ArraySegment<byte>>.Add(ArraySegment<byte> item) { }
            void ICollection<ArraySegment<byte>>.Clear() { }
            bool ICollection<ArraySegment<byte>>.Contains(ArraySegment<byte> item) { return false; }
            void ICollection<ArraySegment<byte>>.CopyTo(ArraySegment<byte>[] array, int arrayIndex) { }
            int ICollection<ArraySegment<byte>>.Count { get { return _data.Count; } }
            bool ICollection<ArraySegment<byte>>.IsReadOnly { get { return true; } }
            bool ICollection<ArraySegment<byte>>.Remove(ArraySegment<byte> item) { return false; }
            #endregion
            #region IEnumerable<ArraySegment<byte>> Members
            public IEnumerator<ArraySegment<byte>> GetEnumerator() {
                for (int i = 0, length = _data.Count; i < length; i++)
                    yield return i == 1 ? _header : new ArraySegment<byte>(_data[i].Bytes);
            }
            #endregion
            #region IEnumerable Members
            IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
            #endregion
        }
        #endregion
        #region DefaultSecsTracer
        sealed class DefaultSecsTracer : ISecsTracer {
            public void TraceMessageIn(SecsMessage msg, int systembyte) {
                Trace.WriteLine("Received Message[id=0x" + systembyte.ToString("X8") + "]");
            }

            public void TraceMessageOut(SecsMessage msg, int systembyte) {
                Trace.WriteLine("Sent Message[id=0x" + systembyte.ToString("X8") + "]");
            }

            public void TraceInfo(string msg) {
                Trace.WriteLine("Info:" + msg);
            }

            public void TraceWarning(string msg) {
                Trace.WriteLine("Warning:" + msg);
            }

            public void TraceError(string msg) {
                Trace.WriteLine("Error:" + msg);
            }
        }
        #endregion
    }

    public enum ConnectionState {
        Connecting,
        Connected,
        Selected,
        Retry
    }
}