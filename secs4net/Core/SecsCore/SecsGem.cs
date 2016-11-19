using Secs4Net.Properties;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Secs4Net
{
    public sealed class SecsGem : IDisposable
    {
        const int EncodeBytePoolMaxArrayLength = 1024*1024;
        const int EncodeBytePoolMaxArrayPerBucket = 500;

        /// <summary>
        /// HSMS connection state changed event
        /// </summary>
        public event EventHandler<ConnectionState> ConnectionChanged;

        /// <summary>
        /// Primary message received event
        /// </summary>
        public event Action<PrimaryMessageWrapper> PrimaryMessageReceived;

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

        public bool IsActive { get; }
        public IPAddress IpAddress { get; }
        public int Port { get; }
        public int DecoderBufferSize { get; private set; }

        private Socket _socket;

        private readonly StreamDecoder _secsDecoder;
        private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();
        private readonly Timer _timer7;	// between socket connected and received Select.req timer
        private readonly Timer _timer8;
        private readonly Timer _timerLinkTest;

        private readonly Func<Task> _startImpl;
        private readonly Action _stopImpl;

        private static readonly SecsMessage ControlMessage = new SecsMessage(0, 0, string.Empty);
        private static readonly DefaultSecsGemLogger DefaultLogger = new DefaultSecsGemLogger();
        private static readonly Pool<IList<ArraySegment<byte>>> EncodedBufferPool
            = new Pool<IList<ArraySegment<byte>>>(p => new List<ArraySegment<byte>>());

        internal static readonly ArrayPool<byte> EncodedBytePool
            = ArrayPool<byte>.Create(EncodeBytePoolMaxArrayLength, EncodeBytePoolMaxArrayPerBucket);

        private readonly SystemByteGenerator _systemByte = new SystemByteGenerator();

        private readonly EventHandler<SocketAsyncEventArgs> _sendControlMessageCompleteHandler;
        private readonly EventHandler<SocketAsyncEventArgs> _sendDataMessageCompleteHandler;

        internal int NewSystemId => _systemByte.New();

        private readonly TaskFactory _taskFactory;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="isActive">passive or active mode</param>
        /// <param name="ip">if active mode it should be remote device address, otherwise local listener address</param>
        /// <param name="port">if active mode it should be remote device listener's port</param>
        /// <param name="receiveBufferSize">Socket receive buffer size</param>
        public SecsGem(bool isActive,
                       IPAddress ip,
                       int port,
                       int receiveBufferSize = 0x4000)
        {
            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port), port, Resources.SecsGemTcpPortMustGreaterThan0);

            _taskFactory = new TaskFactory(TaskScheduler.Default);
            IpAddress = ip ?? throw new ArgumentNullException(nameof(ip));
            Port = port;
            IsActive = isActive;
            DecoderBufferSize = receiveBufferSize;

            _secsDecoder = new StreamDecoder(receiveBufferSize, HandleControlMessage, HandleDataMessage);

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
                    SendControlMessage(MessageType.LinkTestRequest, NewSystemId);
            }, null, Timeout.Infinite, Timeout.Infinite);

            _sendControlMessageCompleteHandler = SendControlMessageCompleteHandler;
            _sendDataMessageCompleteHandler = SendDataMessageCompleteHandler;

            var receiveCompleteEvent = new SocketAsyncEventArgs();
            receiveCompleteEvent.SetBuffer(_secsDecoder.Buffer, _secsDecoder.BufferOffset, _secsDecoder.BufferCount);
            receiveCompleteEvent.Completed += SocketReceiveEventCompleted;
            if (IsActive)
            {
                _startImpl = async () =>
                {
                    var connected = false;
                    do
                    {
                        if (IsDisposed)
                            return;
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (IsDisposed)
                                return;
                            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            await _socket.ConnectAsync(IpAddress, Port).ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (IsDisposed)
                                return;
                            _logger.Error(ex.Message);
                            _logger.Info($"Start T5 Timer: {T5 / 1000} sec.");
                            await Task.Delay(T5);
                        }
                    } while (!connected);

                    CommunicationStateChanging(ConnectionState.Connected);

                    // hook receive event first, because no message will received before 'SelectRequest' send to device
                    if (!_socket.ReceiveAsync(receiveCompleteEvent))
                        SocketReceiveEventCompleted(_socket, receiveCompleteEvent);

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
                    bool connected = false;
                    do
                    {
                        if (IsDisposed)
                            return;
                        CommunicationStateChanging(ConnectionState.Connecting);
                        try
                        {
                            if (IsDisposed)
                                return;
                            _socket = await server.AcceptAsync().ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (IsDisposed)
                                return;
                            _logger.Error(ex.Message);
                            await Task.Delay(2000);
                        }
                    } while (!connected);

                    CommunicationStateChanging(ConnectionState.Connected);
                    if (!_socket.ReceiveAsync(receiveCompleteEvent))
                        SocketReceiveEventCompleted(_socket, receiveCompleteEvent);
                };

                _stopImpl = delegate
                {
                    if (IsDisposed)
                    {
                        server.Dispose();
                    }
                };
            }
        }

        private void SocketReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
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

                if (_socket == null || IsDisposed)
                    return;

                if (!_socket.ReceiveAsync(e))
                    SocketReceiveEventCompleted(sender, e);
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected exception", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        internal static ArraySegment<byte> EncodeHeader(/* readonly */ref MessageHeader header)
            => new ArraySegment<byte>(header.EncodeTo(EncodedBytePool.Rent(10)), 0, 10);

        private void SendControlMessage(MessageType msgType, int systembyte)
        {
            var token = new TaskCompletionSourceToken(ControlMessage, systembyte, false, msgType);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            var header = new MessageHeader
            {
                DeviceId = 0xFFFF,
                MessageType = msgType,
                SystemBytes = systembyte
            };

            var eap = new SocketAsyncEventArgs
            {
                BufferList = ControlMessage.EncodeTo(EncodedBufferPool.Rent(), EncodeHeader(ref header)),
                UserToken = token,
            };
            eap.Completed += _sendControlMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
                SendControlMessageCompleteHandler(_socket, eap);
        }

        private void SendControlMessageCompleteHandler(object o, SocketAsyncEventArgs e)
        {
            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                return;
            }

            ReleaseEncodedBuffer(e.BufferList);

            _logger.Info($"Sent Control Message: {completeToken.MsgType.GetName()}");
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
#if !DISABLE_T6
                if (!completeToken.Task.Wait(T6))
                {
                    _logger.Error($"T6 Timeout: {T6 / 1000} sec.");
                    CommunicationStateChanging(ConnectionState.Retry);
                }
#endif
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }
        }

        private static void ReleaseEncodedBuffer(IList<ArraySegment<byte>> e)
        {
            foreach (var b in e)
                EncodedBytePool.Return(b.Array);

            e.Clear();
            EncodedBufferPool.Return(e);
        }

        private void HandleControlMessage(MessageHeader header)
        {
            if ((byte)header.MessageType % 2 == 0)
            {
                if (_replyExpectedMsgs.TryGetValue(header.SystemBytes, out var ar))
                {
                    ar.SetResult(ControlMessage);
                }
                else
                {
                    _logger.Warning($"Received Unexpected Control Message: {header.MessageType.GetName()}");
                    return;
                }
            }
            _logger.Info($"Receive Control message: {header.MessageType.GetName()}");
            switch (header.MessageType)
            {
                case MessageType.SelectRequest:
                    SendControlMessage(MessageType.SelectResponse, header.SystemBytes);
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
                    SendControlMessage(MessageType.LinkTestResponse, header.SystemBytes);
                    break;
                case MessageType.SeperateRequest:
                    CommunicationStateChanging(ConnectionState.Retry);
                    break;
            }
        }

        internal Task<SecsMessage> SendDataMessageAsync(SecsMessage msg, int systembyte, bool autoDispose = true)
        {
            if (State != ConnectionState.Selected)
                throw new SecsException("Device is not selected");

            var token = new TaskCompletionSourceToken(msg, systembyte, autoDispose);
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

            var eap = new SocketAsyncEventArgs
            {
                BufferList = msg.EncodeTo(EncodedBufferPool.Rent(), EncodeHeader(ref header)),
                UserToken = token,
            };
            eap.Completed += _sendDataMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
                SendDataMessageCompleteHandler(_socket, eap);

            return token.Task;
        }

        private void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
        {
            ReleaseEncodedBuffer(e.BufferList);

            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                CommunicationStateChanging(ConnectionState.Retry);
                return;
            }

            _logger.MessageOut(completeToken.MessageSent, completeToken.Id);
            if (completeToken.AutoDispose)
                completeToken.MessageSent.Dispose();

            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
#if !DISABLE_T3
                if (!completeToken.Task.Wait(T3))
                {
                    _logger.Error($"T3 Timeout[id=0x{completeToken.Id:X8}]: {T3 / 1000} sec.");
                    completeToken.SetException(new SecsException(completeToken.Id, Resources.T3Timeout));
                }
#endif
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }


        }

        private void HandleDataMessage(MessageHeader header, SecsMessage msg)
        {
            if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
            {
                _logger.MessageIn(msg, header.SystemBytes);
                _logger.Warning("Received Unrecognized Device Id Message");
                msg.Dispose();

                SendDataMessageAsync(new SecsMessage(9, 1, false, "UnrecognizedDeviceId", B(header.EncodeTo(new byte[10]))), NewSystemId);

                return;
            }

            if (msg.F % 2 != 0)
            {
                if (msg.S != 9)
                {
                    //Primary message
                    _logger.MessageIn(msg, header.SystemBytes);
                    _taskFactory.StartNew(
                        wrapper => PrimaryMessageReceived?.Invoke((PrimaryMessageWrapper) wrapper),
                        new PrimaryMessageWrapper(this, header, msg));

                    return;
                }
                // Error message systembyte
                unsafe
                {
                    var headerBytes = msg.SecsItem.GetValues<byte>();
                    Array.Reverse(headerBytes, 0, 10);
                    Unsafe.CopyBlock(
                        destination: Unsafe.AsPointer(ref header.SystemBytes),
                        source: Unsafe.AsPointer(ref headerBytes[0]),
                        byteCount: 4);
                }
            }

            // Secondary message
            _logger.MessageIn(msg, header.SystemBytes);
            if (_replyExpectedMsgs.TryGetValue(header.SystemBytes, out var ar))
                ar.HandleReplyMessage(msg);
        }

        private void CommunicationStateChanging(ConnectionState newState)
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
#if !DISABLE_T7
                    _logger.Info($"Start T7 Timer: {T7 / 1000} sec.");
                    _timer7.Change(T7, Timeout.Infinite);
#endif
                    break;
                case ConnectionState.Retry:
                    if (IsDisposed)
                        return;
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

            foreach (var taskCompletionSourceToken in _replyExpectedMsgs.Values)
            {
                if(taskCompletionSourceToken.AutoDispose)
                    taskCompletionSourceToken.MessageSent.Dispose();
            }

            _replyExpectedMsgs.Clear();

            _stopImpl?.Invoke();

            if (_socket == null)
                return;

            if (_socket.Connected)
                _socket.Shutdown(SocketShutdown.Both);

            _socket.Dispose();
            _socket = null;
        }
        public void Start() => _taskFactory.StartNew(_startImpl);

        /// <summary>
        /// Asynchronously send message to device .
        /// </summary>
        /// <param name="msg">primary message</param>
        /// <param name="autoDispose">auto dispose message after message sent.</param>
        /// <returns>secondary message</returns>
        public Task<SecsMessage> SendAsync(SecsMessage msg, bool autoDispose = true)
            => SendDataMessageAsync(msg, NewSystemId, autoDispose);

        private const int DisposalNotStarted = 0;
        private const int DisposalComplete = 1;
        private int _disposeStage;

        public bool IsDisposed => Interlocked.CompareExchange(ref _disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
                return;

            ConnectionChanged = null;
            if (State == ConnectionState.Selected)
                SendControlMessage(MessageType.SeperateRequest, NewSystemId);
            Reset();
            _timer7.Dispose();
            _timer8.Dispose();
            _timerLinkTest.Dispose();
        }

        /// <summary>
        /// remote device endpoint address
        /// </summary>
        public string DeviceAddress => IsActive
            ? IpAddress.ToString()
            : ((IPEndPoint)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        private sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
        {
            internal readonly SecsMessage MessageSent;
            private readonly string _messageName;
            internal readonly int Id;
            internal readonly MessageType MsgType;
            internal readonly bool AutoDispose;

            internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg, int id, bool autoDispose, MessageType msgType = MessageType.DataMessage)
            {
                MessageSent = primaryMessageMsg;
                _messageName = primaryMessageMsg.Name;
                Id = id;
                MsgType = msgType;
                AutoDispose = autoDispose;
            }

            internal void HandleReplyMessage(SecsMessage replyMsg)
            {
                replyMsg.Name = _messageName;
                if (replyMsg.F == 0)
                {
                    SetException(new SecsException(Id, Resources.SxF0));
                    return;
                }

                if (replyMsg.S == 9)
                {
                    switch (replyMsg.F)
                    {
                        case 1:
                            SetException(new SecsException(Id, Resources.S9F1));
                            break;
                        case 3:
                            SetException(new SecsException(Id, Resources.S9F3));
                            break;
                        case 5:
                            SetException(new SecsException(Id, Resources.S9F5));
                            break;
                        case 7:
                            SetException(new SecsException(Id, Resources.S9F7));
                            break;
                        case 9:
                            SetException(new SecsException(Id, Resources.S9F9));
                            break;
                        case 11:
                            SetException(new SecsException(Id, Resources.S9F11));
                            break;
                        case 13:
                            SetException(new SecsException(Id, Resources.S9F13));
                            break;
                        default:
                            SetException(new SecsException(Id, Resources.S9Fy));
                            break;
                    }
                    return;
                }

                SetResult(replyMsg);
            }
        }
    }
}