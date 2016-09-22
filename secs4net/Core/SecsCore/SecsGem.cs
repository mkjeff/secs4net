using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Secs4Net.Properties;

namespace Secs4Net
{
    public sealed class SecsGem : IDisposable
    {
        /// <summary>
        /// HSMS connection state changed event
        /// </summary>
        public event EventHandler<ConnectionState> ConnectionChanged;

        /// <summary>
        /// Primary message received event
        /// </summary>
        public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceived;


        public ISecsGemLogger Logger
        {
            get { return _logger; }
            set { _logger = value ?? DefaultLogger; }
        }

        /// <summary>
        /// Connection state
        /// </summary>
        public ConnectionState State { get; private set; }

        /// <summary>
        /// Device Id.
        /// </summary>
        public ushort DeviceId { get; set; } = 0;

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

        private int _linkTestInterval = 60000;
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

        readonly StreamDecoder _secsDecoder;
        readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();
        ISecsGemLogger _logger = DefaultLogger;
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

        internal int NewSystemId => _systemByte.New();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="isActive">passive or active mode</param>
        /// <param name="ip">if active mode it should be remote device address, otherwise local listener address</param>
        /// <param name="port">if active mode it should be remote deivice listener's port</param>
        /// <param name="receiveBufferSize">Socket receive buffer size</param>
        public SecsGem(bool isActive, IPAddress ip, int port, int receiveBufferSize = 0x4000)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port), port, $"port number must greater than 0");

            _ip = ip;
            _port = port;
            _isActive = isActive;
            _secsDecoder = new StreamDecoder(_logger, HandleControlMessage, HandleDataMessage);

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
                    SendControlMessage(MessageType.LinkTestRequest, NewSystemId);
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
                        if (_isDisposed)
                            return;
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (_isDisposed)
                                return;
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

                    SendControlMessage(MessageType.SelectRequest, NewSystemId);
                };

                //_stopImpl = delegate { };
                #endregion
            }
            else
            {
                #region Passive Impl
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(_ip, _port));
                server.Listen(0);

                _startImpl = async () =>
                {
                    bool connected = false;
                    do
                    {
                        if (_isDisposed)
                            return;
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (_isDisposed)
                                return;
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
            var token = new TaskCompletionSourceToken(ControlMessage, systembyte, msgType);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            var eap = new SocketAsyncEventArgs
            {
                BufferList = new List<ArraySegment<byte>>(2) {
                    ControlMessageLengthBytes,
                    new ArraySegment<byte>(new MessageHeader{
                        DeviceId = 0xFFFF,
                        MessageType = msgType,
                        SystemBytes = systembyte
                    }.Bytes)
                },
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

        void HandleControlMessage(MessageHeader header)
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

        internal Task<SecsMessage> SendDataMessageAsync(SecsMessage msg, int systembyte)
        {
            if (State != ConnectionState.Selected)
                throw new SecsException("Device is not selected");

            var token = new TaskCompletionSourceToken(msg, systembyte);
            if (msg.ReplyExpected)
                _replyExpectedMsgs[systembyte] = token;

            var header = new MessageHeader
            {
                S = msg.S,
                F = msg.F,
                ReplyExpected = msg.ReplyExpected,
                DeviceId = DeviceId,
                SystemBytes = systembyte
            };

            var bufferList = msg.RawDatas.Value;
            bufferList[1] = new ArraySegment<byte>(header.Bytes);
            var eap = new SocketAsyncEventArgs
            {
                BufferList = bufferList,
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

        void HandleDataMessage(MessageHeader header, SecsMessage msg)
        {
            int systembyte = header.SystemBytes;

            if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
            {
                _logger.TraceMessageIn(msg, systembyte);
                _logger.TraceWarning("Received Unrecognized Device Id Message");
                SendDataMessageAsync(new SecsMessage(9, 1, false, "Unrecognized Device Id", Item.B(header.Bytes)), NewSystemId);
                return;
            }

            if (msg.F % 2 != 0)
            {
                if (msg.S != 9)
                {
                    //Primary message
                    _logger.TraceMessageIn(msg, systembyte);
                    PrimaryMessageReceived?.Invoke(this, new PrimaryMessageWrapper(this, header, msg));
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
            ConnectionChanged?.Invoke(this, State);

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
            _stopImpl?.Invoke();
        }
        public void Start() => new TaskFactory(TaskScheduler.Default).StartNew(_startImpl);

        /// <summary>
        /// Asynchronously send message to device .
        /// </summary>
        /// <param name="msg">primary message</param>
        /// <returns>secondary message</returns>
        public Task<SecsMessage> SendAsync(SecsMessage msg) => SendDataMessageAsync(msg, NewSystemId);

        volatile bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                ConnectionChanged = null;
                if (State == ConnectionState.Selected)
                    SendControlMessage(MessageType.SeperateRequest, NewSystemId);
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
    }
}