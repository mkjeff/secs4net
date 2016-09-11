using Secs4Net.Properties;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class SecsGem
    {
        /// <summary>
        /// HSMS connection state changed event
        /// </summary>
        public event EventHandler ConnectionChanged;

        /// <summary>
        /// Primary message received event
        /// </summary>
        public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceived;


        /// <summary>
        /// Connection state
        /// </summary>
        public ConnectionState State { get; private set; }

        /// <summary>
        /// Device Id.
        /// </summary>
        public short DeviceId { get; set; } = 0;

        /// <summary>
        /// T3 timer interval 
        /// </summary>
        public int T3 { get; set; } = 45000;

        /// <summary>
        /// T5 timer interval
        /// </summary>
        public int T5 { get; set; } = 10000;

        /// <summary>
        /// T6 timer interval
        /// </summary>
        public int T6 { get; set; } = 5000;

        /// <summary>
        /// T7 timer interval
        /// </summary>
        public int T7 { get; set; } = 10000;

        /// <summary>
        /// T8 timer interval
        /// </summary>
        public int T8 { get; set; } = 5000;

        private int _linkTestInterval = 60000;

        /// <summary>
        /// Linking test timer interval
        /// </summary>
        public int LinkTestInterval
        {
            get { return _linkTestInterval; }
            set
            {
                if (_linkTestEnable)
                {
                    _linkTestInterval = value;
                    _timerLinkTest.Change(0, _linkTestInterval);
                }
            }
        }

        private bool _linkTestEnable;

        /// <summary>
        /// get or set linking test timer enable or not 
        /// </summary>
        public bool LinkTestEnable
        {
            get { return _linkTestEnable; }
            set
            {
                if (_linkTestEnable == value)
                    return;

                _linkTestEnable = value;
                if (_linkTestEnable)
                    _timerLinkTest.Change(0, LinkTestInterval);
                else
                    _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        readonly bool _isActive;
        readonly IPAddress _ip;
        readonly int _port;
        Socket _socket;

        readonly SecsDecoder _secsDecoder;
        readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();
        readonly ISecsGemLogger _logger;
        readonly Timer _timer7;	// between socket connected and received Select.req timer
        readonly Timer _timer8;
        readonly Timer _timerLinkTest;

        readonly Func<Task> _startImpl;
        readonly Action _stopImpl;

        static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
        static readonly ArraySegment<byte> ControlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });
        static readonly DefaultSecsGemLogger DefaultLogger = new DefaultSecsGemLogger();
        readonly SystemByteGenerator _systemByte = new SystemByteGenerator();

        readonly EventHandler<SocketAsyncEventArgs> _sendControlMessageCompleteHandler;
        readonly EventHandler<SocketAsyncEventArgs> _sendDataMessageCompleteHandler;

        void DefaultPrimaryMessageHandler(SecsMessage primary, Action<SecsMessage> reply) => reply(null);

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="isActive">passive or active mode</param>
        /// <param name="ip">if active mode it should be remote device address, otherwise local listener address</param>
        /// <param name="port">if active mode it should be remote deivice listener's port</param>
        /// <param name="logger">log tracer</param>
        /// <param name="receiveBufferSize">Socket receive buffer size</param>
        public SecsGem(bool isActive, IPAddress ip, int port, ISecsGemLogger logger = null, int receiveBufferSize = 0x4000, ISecsMessageFormatter formatter=null)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port), port, $"port number must greater than 0");

            _ip = ip;
            _port = port;
            _isActive = isActive;
            _logger = logger ?? DefaultLogger;
            _secsDecoder = new SecsDecoder(_logger, HandleControlMessage, HandleDataMessage);

            #region Timer Action
            _timer7 = new Timer(delegate
            {
                _logger.TraceError("T7 Timeout");
                CommunicationStateChanging(ConnectionState.Retry);
            }, null, Timeout.Infinite, Timeout.Infinite);

            _timer8 = new Timer(delegate
            {
                _logger.TraceError("T8 Timeout");
                CommunicationStateChanging(ConnectionState.Retry);
            }, null, Timeout.Infinite, Timeout.Infinite);

            _timerLinkTest = new Timer(delegate
            {
                if (State == ConnectionState.Selected)
                    SendControlMessage(MessageType.LinkTestRequest, _systemByte.New());
            }, null, Timeout.Infinite, Timeout.Infinite);
            #endregion
            var receiveBuffer = new byte[receiveBufferSize < 0x4000 ? 0x4000 : receiveBufferSize];
            var receiveCompleteEvent = new SocketAsyncEventArgs();
            receiveCompleteEvent.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
            receiveCompleteEvent.Completed += ReceiveEventCompleted;
            if (_isActive)
            {
                #region Active Impl

                _startImpl = async () =>
                {
                    bool connected = false;
                    do
                    {
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            await _socket.ConnectAsync(_ip, _port).ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (_isDisposed)
                                return;
                            _logger.TraceError(ex.Message);
                            _logger.TraceInfo($"Start T5 Timer: waiting {T5 / 1000} second.");
                            await Task.Delay(T5);
                        }
                    } while (!connected);

                    CommunicationStateChanging(ConnectionState.Connected);

                    // hook receive event first, because no message will received before 'SelectRequest' send to device
                    if (!_socket.ReceiveAsync(receiveCompleteEvent))
                        ReceiveEventCompleted(_socket, receiveCompleteEvent);

                    SendControlMessage(MessageType.SelectRequest, _systemByte.New());
                };

                _stopImpl = delegate { };
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
                    bool connected = false;
                    do
                    {
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            _socket = await server.AcceptAsync().ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (_isDisposed)
                                return;
                            _logger.TraceError(ex.Message);
                            await Task.Delay(2000);
                        }
                    } while (!connected);

                    CommunicationStateChanging(ConnectionState.Connected);
                    if (!_socket.ReceiveAsync(receiveCompleteEvent))
                        ReceiveEventCompleted(_socket, receiveCompleteEvent);
                };

                _stopImpl = delegate
                {
                    if (_isDisposed)
                    {
                        server.Dispose();
                    }
                };
                #endregion
            }

            _sendControlMessageCompleteHandler = SendControlMessageCompleteHandler;
            _sendDataMessageCompleteHandler = SendDataMessageCompleteHandler;
        }

        private void ReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                var ex = new SocketException((int)e.SocketError);
                _logger.TraceError($"RecieveComplete socket error:{ex.Message + ex}, ErrorCode:{ex.SocketErrorCode}", ex);
                CommunicationStateChanging(ConnectionState.Retry);
                return;
            }

            try
            {
                _timer8.Change(Timeout.Infinite, Timeout.Infinite);
                if (e.BytesTransferred == 0)
                {
                    _logger.TraceError("Received 0 byte.");
                    CommunicationStateChanging(ConnectionState.Retry);
                    return;
                }
                else if (_secsDecoder.Decode(e.Buffer, 0, e.BytesTransferred))
                {
                    _logger.TraceInfo("StartAsync T8 Timer");
                    _timer8.Change(T8, Timeout.Infinite);
                }

                if (!_socket.ReceiveAsync(e))
                    ReceiveEventCompleted(sender, e);
            }
            catch (NullReferenceException ex)
            {
                _logger.TraceWarning("unexpected NullReferenceException:" + ex);
            }
            catch (Exception ex)
            {
                _logger.TraceError("unexpected exception", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        void SendControlMessage(MessageType msgType, int systembyte)
        {
            var header = new Header(new byte[10]) { MessageType = msgType, SystemBytes = systembyte };
            header.Bytes[0] = 0xFF;
            header.Bytes[1] = 0xFF;
            var token = new TaskCompletionSourceToken(ControlMessage, systembyte, msgType);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            var eap = new SocketAsyncEventArgs
            {
                BufferList = new List<ArraySegment<byte>>(2) { ControlMessageLengthBytes, new ArraySegment<byte>(header.Bytes) },
                UserToken = token,
            };
            eap.Completed += _sendControlMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
                SendControlMessageCompleteHandler(_socket, eap);
        }

        void SendControlMessageCompleteHandler(object o, SocketAsyncEventArgs e)
        {
            var completeToken = e.UserToken as TaskCompletionSourceToken;
            if (completeToken == null)
                throw new InvalidOperationException("SocketAsyncEventArgs.UserToken is not TaskCompletionSourceToken.");

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                return;
            }

            _logger.TraceInfo("Sent Control Message: " + completeToken.MsgType);
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T6))
                {
                    _logger.TraceError("T6 Timeout");
                    CommunicationStateChanging(ConnectionState.Retry);
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }
        }

        void HandleControlMessage(Header header)
        {
            int systembyte = header.SystemBytes;
            if ((byte)header.MessageType % 2 == 0)
            {
                TaskCompletionSourceToken ar;
                if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
                {
                    ar.SetResult(ControlMessage);
                }
                else
                {
                    _logger.TraceWarning("Received Unexpected Control Message: " + header.MessageType);
                    return;
                }
            }
            _logger.TraceInfo("Receive Control message: " + header.MessageType);
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
                            _logger.TraceError("Communication Already Active.");
                            break;
                        case 2:
                            _logger.TraceError("Connection Not Ready.");
                            break;
                        case 3:
                            _logger.TraceError("Connection Exhaust.");
                            break;
                        default:
                            _logger.TraceError("Connection Status Is Unknown.");
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

        Task<SecsMessage> SendDataMessageAsync(SecsMessage msg, int systembyte)
        {
            if (State != ConnectionState.Selected)
                throw new SecsException("Device is not selected");

            var token = new TaskCompletionSourceToken(msg, systembyte);
            if (msg.ReplyExpected)
                _replyExpectedMsgs[systembyte] = token;

            var header = new Header(new byte[10])
            {
                S = msg.S,
                F = msg.F,
                ReplyExpected = msg.ReplyExpected,
                DeviceId = DeviceId,
                SystemBytes = systembyte
            };
            msg.RawDatas[1] = new ArraySegment<byte>(header.Bytes);
            var eap = new SocketAsyncEventArgs
            {
                BufferList = msg.RawDatas,
                UserToken = token,
            };
            eap.Completed += _sendDataMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
                SendDataMessageCompleteHandler(_socket, eap);

            return token.Task;
        }

        void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
        {
            var completeToken = e.UserToken as TaskCompletionSourceToken;
            if (completeToken == null)
                return;

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                CommunicationStateChanging(ConnectionState.Retry);
                return;
            }

            _logger.TraceMessageOut(completeToken.MessageSent, completeToken.Id);
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T3))
                {
                    _logger.TraceError($"T3 Timeout[id=0x{completeToken.Id:X8}]");
                    completeToken.SetException(new SecsException(completeToken.MessageSent, Resources.T3Timeout));
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }
        }

        void HandleDataMessage(Header header, SecsMessage msg)
        {
            int systembyte = header.SystemBytes;

            if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
            {
                _logger.TraceMessageIn(msg, systembyte);
                _logger.TraceWarning("Received Unrecognized Device Id Message");
                SendDataMessageAsync(new SecsMessage(9, 1, false, "Unrecognized Device Id", Item.B(header.Bytes)), _systemByte.New());
                return;
            }

            if (msg.F % 2 != 0)
            {
                if (msg.S != 9)
                {
                    //Primary message
                    _logger.TraceMessageIn(msg, systembyte);
                    PrimaryMessageReceived?.Invoke(this, new PrimaryMessageWrapper(systembyte, msg, secondary =>
                    {
                        if (!header.ReplyExpected || State != ConnectionState.Selected)
                            return ;

                        secondary = secondary ?? new SecsMessage(9, 7, false, "Unknown Message", Item.B(header.Bytes));
                        secondary.ReplyExpected = false;

                        SendDataMessageAsync(secondary, secondary.S == 9 ? _systemByte.New() : header.SystemBytes);
                    }));
                    return;
                }
                // Error message
                var headerBytes = (byte[])msg.SecsItem;
                systembyte = BitConverter.ToInt32(new[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
            }

            // Secondary message
            _logger.TraceMessageIn(msg, systembyte);
            TaskCompletionSourceToken ar;
            if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
                ar.HandleReplyMessage(msg);
        }

        void CommunicationStateChanging(ConnectionState newState)
        {
            State = newState;
            ConnectionChanged?.Invoke(this, EventArgs.Empty);

            switch (State)
            {
                case ConnectionState.Selected:
                    _timer7.Change(Timeout.Infinite, Timeout.Infinite);
                    _logger.TraceInfo("Stop T7 Timer");
                    break;
                case ConnectionState.Connected:
                    _logger.TraceInfo("Start T7 Timer");
                    _timer7.Change(T7, Timeout.Infinite);
                    break;
                case ConnectionState.Retry:
                    if (_isDisposed)
                        return;
                    Reset();
                    Task.Factory.StartNew(_startImpl);
                    break;
            }
        }

        void Reset()
        {
            _timer7.Change(Timeout.Infinite, Timeout.Infinite);
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
            _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
            _secsDecoder.Reset();
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Dispose();
                _socket = null;
            }
            _replyExpectedMsgs.Clear();
            _stopImpl.Invoke();
        }
        public Task StartAsync() => _startImpl();

        /// <summary>
        /// Asynchronously send message to device .
        /// </summary>
        /// <param name="msg">primary message</param>
        /// <returns>secondary message</returns>
        public Task<SecsMessage> SendAsync(SecsMessage msg) => SendDataMessageAsync(msg, _systemByte.New());

        volatile bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                ConnectionChanged = null;
                if (State == ConnectionState.Selected)
                    SendControlMessage(MessageType.SeperateRequest, _systemByte.New());
                Reset();
                _timer7.Dispose();
                _timer8.Dispose();
                _timerLinkTest.Dispose();
            }
        }

        /// <summary>
        /// remote device endpoint address
        /// </summary>
        public string DeviceAddress => _isActive
            ? _ip.ToString()
            : ((IPEndPoint)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        #region Async Token        

        sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
        {
            internal readonly SecsMessage MessageSent;
            internal readonly int Id;
            internal readonly MessageType MsgType;

            internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg, int id, MessageType msgType = MessageType.DataMessage)
            {
                MessageSent = primaryMessageMsg;
                Id = id;
                MsgType = msgType;
            }

            internal void HandleReplyMessage(SecsMessage replyMsg)
            {
                replyMsg.Name = MessageSent.Name;
                if (replyMsg.F == 0)
                {
                    SetException(new SecsException(MessageSent, Resources.SxF0));
                    return;
                }

                if (replyMsg.S == 9)
                {
                    switch (replyMsg.F)
                    {
                        case 1:
                            SetException(new SecsException(MessageSent, Resources.S9F1));
                            break;
                        case 3:
                            SetException(new SecsException(MessageSent, Resources.S9F3));
                            break;
                        case 5:
                            SetException(new SecsException(MessageSent, Resources.S9F5));
                            break;
                        case 7:
                            SetException(new SecsException(MessageSent, Resources.S9F7));
                            break;
                        case 9:
                            SetException(new SecsException(MessageSent, Resources.S9F9));
                            break;
                        case 11:
                            SetException(new SecsException(MessageSent, Resources.S9F11));
                            break;
                        case 13:
                            SetException(new SecsException(MessageSent, Resources.S9F13));
                            break;
                        default:
                            SetException(new SecsException(MessageSent, Resources.S9Fy));
                            break;
                    }
                    return;
                }

                SetResult(replyMsg);
            }
        }        
        
        #endregion
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

            readonly ISecsGemLogger _logger;
            /// <summary>
            /// decode pipeline
            /// </summary>
            readonly Decoder[] _decoders;
            readonly Action<Header, SecsMessage> _dataMsgHandler;
            readonly Action<Header> _controlMsgHandler;

            internal SecsDecoder(ISecsGemLogger logger, Action<Header> controlMsgHandler, Action<Header, SecsMessage> dataMsgHandler)
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
    }
}