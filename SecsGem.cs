using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
        public event EventHandler<PrimaryMessageWrapper> PrimaryMessageReceived = DefaultPrimaryMessageReceived;
        private static void DefaultPrimaryMessageReceived(object sender, PrimaryMessageWrapper _) { }

        private ISecsGemLogger _logger = DefaultLogger;
        public ISecsGemLogger Logger
        {
            get => _logger;
            set => _logger = value ?? DefaultLogger;
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
            get => _linkTestInterval;
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
            get => _linkTestEnable;
            set
            {
                if (_linkTestEnable == value)
                {
                    return;
                }

                _linkTestEnable = value;
                if (_linkTestEnable)
                {
                    _timerLinkTest.Change(0, LinkTestInterval);
                }
                else
                {
                    _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        public bool IsActive { get; }
        public IPAddress IpAddress { get; }
        public int Port { get; }
        public int DecoderBufferSize { get; private set; }

        private const int DisposalNotStarted = 0;
        private const int DisposalComplete = 1;
        private int _disposeStage;

        public bool IsDisposed => Interlocked.CompareExchange(ref _disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;
        /// <summary>
        /// remote device endpoint address
        /// </summary>
        public string DeviceIpAddress => IsActive
            ? IpAddress.ToString()
            : ((IPEndPoint)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        private Socket _socket;

        private readonly StreamDecoder _secsDecoder;
        private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();
        private readonly Timer _timer7;	// between socket connected and received Select.req timer
        private readonly Timer _timer8;
        private readonly Timer _timerLinkTest;

        private readonly Func<Task> _startImpl;
        private readonly Action _stopImpl;

        private static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
        private static readonly ArraySegment<byte> ControlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });
        private static readonly DefaultSecsGemLogger DefaultLogger = new DefaultSecsGemLogger();
        private readonly SystemByteGenerator _systemByte = new SystemByteGenerator();

        private readonly EventHandler<SocketAsyncEventArgs> _sendControlMessageCompleteHandler;
        private readonly EventHandler<SocketAsyncEventArgs> _sendDataMessageCompleteHandler;

        internal int NewSystemId => _systemByte.New();

        private readonly TaskFactory _taskFactory = new TaskFactory(TaskScheduler.Default);

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="isActive">passive or active mode</param>
        /// <param name="ip">if active mode it should be remote device address, otherwise local listener address</param>
        /// <param name="port">if active mode it should be remote device listener's port</param>
        /// <param name="receiveBufferSize">Socket receive buffer size</param>
        public SecsGem(bool isActive, IPAddress ip, int port, int receiveBufferSize = 0x4000)
        {
            if (port <= 0)
            {
                port = 0;
            }

            _sendControlMessageCompleteHandler = SendControlMessageCompleteHandler;
            _sendDataMessageCompleteHandler = SendDataMessageCompleteHandler;
            _secsDecoder = new StreamDecoder(receiveBufferSize, HandleControlMessage, HandleDataMessage);

            IpAddress = ip;
            Port = port;
            IsActive = isActive;
            DecoderBufferSize = receiveBufferSize;

            #region Timer Action
            _timer7 = new Timer(delegate
            {
                _logger.Error($"T7 Timeout: {T7 / 1000} sec.");
                CommunicationStateChanging(ConnectionState.Retry);
            }, null, Timeout.Infinite, Timeout.Infinite);

            _timer8 = new Timer(delegate
            {
                _logger.Error($"T8 Timeout: {T8 / 1000} sec.");
                CommunicationStateChanging(ConnectionState.Retry);
            }, null, Timeout.Infinite, Timeout.Infinite);

            _timerLinkTest = new Timer(delegate
            {
                if (State == ConnectionState.Selected)
                {
                    SendControlMessage(MessageType.LinkTestRequest, NewSystemId);
                }
            }, null, Timeout.Infinite, Timeout.Infinite);
            #endregion

            if (IsActive)
            {
                _startImpl = async () =>
                {
                    var connected = false;
                    do
                    {
                        if (IsDisposed)
                        {
                            return;
                        }

                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            await _socket.ConnectAsync(IpAddress, Port).ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _logger.Error(ex.Message);
                            _logger.Info($"Start T5 Timer: {T5 / 1000} sec.");
                            await Task.Delay(T5);
                        }
                    } while (!connected);

                    // hook receive event first, because no message will received before 'SelectRequest' send to device
                    StartSocketReceive();
                    SendControlMessage(MessageType.SelectRequest, NewSystemId);
                };

                //_stopImpl = delegate { };
            }
            else
            {
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IpAddress, Port));
                server.Listen(0);

                _startImpl = async () =>
                {
                    var connected = false;
                    do
                    {
                        if (IsDisposed)
                        {
                            return;
                        }

                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _socket = await server.AcceptAsync().ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _logger.Error(ex.Message);
                            await Task.Delay(2000).ConfigureAwait(false);
                        }
                    } while (!connected);

                    StartSocketReceive();
                };

                _stopImpl = delegate
                {
                    if (IsDisposed)
                    {
                        server.Dispose();
                    }
                };
            }

            void StartSocketReceive()
            {
                CommunicationStateChanging(ConnectionState.Connected);

                var receiveCompleteEvent = new SocketAsyncEventArgs();
                receiveCompleteEvent.SetBuffer(_secsDecoder.Buffer, _secsDecoder.BufferOffset, _secsDecoder.BufferCount);
                receiveCompleteEvent.Completed += SocketReceiveEventCompleted;

                if (!_socket.ReceiveAsync(receiveCompleteEvent))
                {
                    SocketReceiveEventCompleted(_socket, receiveCompleteEvent);
                }

                void SocketReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
                {
                    if (e.SocketError != SocketError.Success)
                    {
                        var ex = new SocketException((int)e.SocketError);
                        _logger.Error($"RecieveComplete socket error:{ex.Message}, ErrorCode:{ex.SocketErrorCode}", ex);
                        CommunicationStateChanging(ConnectionState.Retry);
                        return;
                    }

                    try
                    {
                        _timer8.Change(Timeout.Infinite, Timeout.Infinite);
                        var receivedCount = e.BytesTransferred;
                        if (receivedCount == 0)
                        {
                            _logger.Error("Received 0 byte.");
                            CommunicationStateChanging(ConnectionState.Retry);
                            return;
                        }

                        if (_secsDecoder.Decode(receivedCount))
                        {
#if !DISABLE_T8
                            _logger.Debug($"Start T8 Timer: {T8 / 1000} sec.");
                            _timer8.Change(T8, Timeout.Infinite);
#endif
                        }

                        if (_secsDecoder.Buffer.Length != DecoderBufferSize)
                        {
                            // buffer size changed
                            e.SetBuffer(_secsDecoder.Buffer, _secsDecoder.BufferOffset, _secsDecoder.BufferCount);
                            DecoderBufferSize = _secsDecoder.Buffer.Length;
                        }
                        else
                        {
                            e.SetBuffer(_secsDecoder.BufferOffset, _secsDecoder.BufferCount);
                        }

                        if (_socket is null || IsDisposed)
                        {
                            return;
                        }

                        if (!_socket.ReceiveAsync(e))
                        {
                            SocketReceiveEventCompleted(sender, e);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Unexpected exception", ex);
                        CommunicationStateChanging(ConnectionState.Retry);
                    }
                }
            }

            void HandleControlMessage(MessageHeader header)
            {
                var systembyte = header.SystemBytes;
                if ((byte)header.MessageType % 2 == 0)
                {
                    if (_replyExpectedMsgs.TryGetValue(systembyte, out var ar))
                    {
                        ar.SetResult(ControlMessage);
                    }
                    else
                    {
                        _logger.Warning("Received Unexpected Control Message: " + header.MessageType);
                        return;
                    }
                }
                _logger.Info("Receive Control message: " + header.MessageType);
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
                                _logger.Error("Communication Already Active.");
                                break;
                            case 2:
                                _logger.Error("Connection Not Ready.");
                                break;
                            case 3:
                                _logger.Error("Connection Exhaust.");
                                break;
                            default:
                                _logger.Error("Connection Status Is Unknown.");
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
            
            void HandleDataMessage(MessageHeader header, SecsMessage msg)
            {
                var systembyte = header.SystemBytes;

                if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
                {
                    _logger.MessageIn(msg, systembyte);
                    _logger.Warning("Received Unrecognized Device Id Message");
                    SendDataMessageAsync(new SecsMessage(9, 1, "Unrecognized Device Id", Item.B(header.EncodeTo(new byte[10])), replyExpected: false), NewSystemId);
                    return;
                }

                if (msg.F % 2 != 0)
                {
                    if (msg.S != 9)
                    {
                        //Primary message
                        _logger.MessageIn(msg, systembyte);
                        _taskFactory.StartNew(
                            wrapper => PrimaryMessageReceived(this, Unsafe.As<PrimaryMessageWrapper>(wrapper)),
                            new PrimaryMessageWrapper(this, header, msg));

                        return;
                    }
                    // Error message
                    var headerBytes = msg.SecsItem.GetValues<byte>();
                    systembyte = BitConverter.ToInt32(new[] { headerBytes[9], headerBytes[8], headerBytes[7], headerBytes[6] }, 0);
                }

                // Secondary message
                _logger.MessageIn(msg, systembyte);
                if (_replyExpectedMsgs.TryGetValue(systembyte, out var ar))
                {
                    ar.HandleReplyMessage(msg);
                }
            }
        }

        private void SendControlMessage(in MessageType msgType, in int systembyte)
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
                    new ArraySegment<byte>(new MessageHeader(
                        deviceId: 0xFFFF,
                        messageType: msgType,
                        systemBytes: systembyte
                    ).EncodeTo(new byte[10]))
                },
                UserToken = token,
            };
            eap.Completed += _sendControlMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
            {
                SendControlMessageCompleteHandler(_socket, eap);
            }
        }

        private void SendControlMessageCompleteHandler(object o, SocketAsyncEventArgs e)
        {
            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                return;
            }

            _logger.Info("Sent Control Message: " + completeToken.MsgType);
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T6))
                {
                    _logger.Error($"T6 Timeout: {T6 / 1000} sec.");
                    CommunicationStateChanging(ConnectionState.Retry);
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out _);
            }
        }

        internal Task<SecsMessage> SendDataMessageAsync(in SecsMessage msg, in int systembyte)
        {
            if (State != ConnectionState.Selected)
            {
                throw new SecsException("Device is not selected");
            }

            var token = new TaskCompletionSourceToken(msg, systembyte, MessageType.DataMessage);
            if (msg.ReplyExpected)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            var header = new MessageHeader
            (
                s: msg.S,
                f: msg.F,
                replyExpected: msg.ReplyExpected,
                deviceId: DeviceId,
                systemBytes: systembyte
            );

            var bufferList = msg.RawDatas.Value;
            bufferList[1] = new ArraySegment<byte>(header.EncodeTo(new byte[10]));
            var eap = new SocketAsyncEventArgs
            {
                BufferList = bufferList,
                UserToken = token,
            };
            eap.Completed += _sendDataMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
            {
                SendDataMessageCompleteHandler(_socket, eap);
            }

            return token.Task;
        }

        private void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
        {
            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                CommunicationStateChanging(ConnectionState.Retry);
                return;
            }

            _logger.MessageOut(completeToken.MessageSent, completeToken.Id);

            if (!_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                completeToken.SetResult(null);
                return;
            }

            try
            {
                if (!completeToken.Task.Wait(T3))
                {
                    _logger.Error($"T3 Timeout[id=0x{completeToken.Id:X8}]: {T3 / 1000} sec.");
                    completeToken.SetException(new SecsException(completeToken.MessageSent, Resources.T3Timeout));
                }
            }
            catch (AggregateException) { }
            finally
            {
                _replyExpectedMsgs.TryRemove(completeToken.Id, out _);
            }
        }

        private void CommunicationStateChanging(in ConnectionState newState)
        {
            State = newState;
            ConnectionChanged?.Invoke(this, State);

            switch (State)
            {
                case ConnectionState.Selected:
                    _timer7.Change(Timeout.Infinite, Timeout.Infinite);
                    _logger.Info("Stop T7 Timer");
                    break;
                case ConnectionState.Connected:
#if !DISABLE_TIMER
                    _logger.Info($"Start T7 Timer: {T7 / 1000} sec.");
                    _timer7.Change(T7, Timeout.Infinite);
#endif
                    break;
                case ConnectionState.Retry:
                    if (IsDisposed)
                    {
                        return;
                    }

                    Reset();
                    Task.Factory.StartNew(_startImpl);
                    break;
            }
        }

        private void Reset()
        {
            _timer7.Change(Timeout.Infinite, Timeout.Infinite);
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
            _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
            _secsDecoder.Reset();
            _replyExpectedMsgs.Clear();
            _stopImpl?.Invoke();

            if (_socket is null)
            {
                return;
            }

            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
            }

            _socket.Dispose();
            _socket = null;
        }

        public void Start() => new TaskFactory(TaskScheduler.Default).StartNew(_startImpl);

        /// <summary>
        /// Asynchronously send message to device .
        /// </summary>
        /// <param name="msg">primary message</param>
        /// <returns>secondary message</returns>
        public Task<SecsMessage> SendAsync(SecsMessage msg) => SendDataMessageAsync(msg, NewSystemId);

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
            {
                return;
            }

            ConnectionChanged = null;
            if (State == ConnectionState.Selected)
            {
                SendControlMessage(MessageType.SeperateRequest, NewSystemId);
            }

            Reset();
            _timer7.Dispose();
            _timer8.Dispose();
            _timerLinkTest.Dispose();
        }



        private sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
        {
            internal readonly SecsMessage MessageSent;
            internal readonly int Id;
            internal readonly MessageType MsgType;

            internal TaskCompletionSourceToken(in SecsMessage primaryMessageMsg, in int id, in MessageType msgType)
            {
                MessageSent = primaryMessageMsg;
                Id = id;
                MsgType = msgType;
            }

            internal void HandleReplyMessage(in SecsMessage replyMsg)
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
    }
}