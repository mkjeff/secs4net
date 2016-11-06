using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Secs4Net.Properties;
using static Secs4Net.Item;

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

        private ISecsGemLogger _logger = DefaultLogger;
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
        static readonly Pool<IList<ArraySegment<byte>>> EncoderPool
            = new Pool<IList<ArraySegment<byte>>>(1000, p => new List<ArraySegment<byte>>());

        private static readonly ArrayPool<byte> HeaderArrayPool = ArrayPool<byte>.Create(10, 100);

        private readonly SystemByteGenerator _systemByte = new SystemByteGenerator();

        private readonly EventHandler<SocketAsyncEventArgs> _sendControlMessageCompleteHandler;
        private readonly EventHandler<SocketAsyncEventArgs> _sendDataMessageCompleteHandler;

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
                throw new ArgumentOutOfRangeException(nameof(port), port, Resources.SecsGemTcpPortMustGreaterThan0);

            IpAddress = ip;
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
            receiveCompleteEvent.Completed += ReceiveEventCompleted;
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
                        ReceiveEventCompleted(_socket, receiveCompleteEvent);

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
                        ReceiveEventCompleted(_socket, receiveCompleteEvent);
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

        private void ReceiveEventCompleted(object sender, SocketAsyncEventArgs e)
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
#if !DISABLE_TIMER
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
                    ReceiveEventCompleted(sender, e);
            }
            catch (NullReferenceException ex)
            {
                _logger.Error("Unexpected NullReferenceException", ex);
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected exception", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        internal static ArraySegment<byte> EncodeHeader(ref MessageHeader header)
            => new ArraySegment<byte>(header.EncodeTo(HeaderArrayPool.Rent(10)));

        private void SendControlMessage(MessageType msgType, int systembyte)
        {
            var token = new TaskCompletionSourceToken(ControlMessage, systembyte, false, msgType);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }
            var bufferList = EncoderPool.Acquire();

            bufferList.Add(GetEmptyDataMessageLengthBytes());

            var header = new MessageHeader
            {
                DeviceId = 0xFFFF,
                MessageType = msgType,
                SystemBytes = systembyte
            };

            bufferList.Add(EncodeHeader(ref header));

            var eap = new SocketAsyncEventArgs
            {
                BufferList = bufferList,
                UserToken = token,
            };
            eap.Completed += _sendControlMessageCompleteHandler;
            if (!_socket.SendAsync(eap))
                SendControlMessageCompleteHandler(_socket, eap);
        }

        internal static ArraySegment<byte> GetEmptyDataMessageLengthBytes()
        {
            var lengthBytes = ArrayPool<byte>.Shared.Rent(4);
            lengthBytes[0] = 0;
            lengthBytes[1] = 0;
            lengthBytes[2] = 0;
            lengthBytes[3] = 10;
            return new ArraySegment<byte>(lengthBytes, 0, 4);
        }

        private void SendControlMessageCompleteHandler(object o, SocketAsyncEventArgs e)
        {
            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                return;
            }

            ReleaseEncoderBuffer(e.BufferList);

            _logger.Info($"Sent Control Message: {completeToken.MsgType.GetName()}");
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T6))
                {
                    _logger.Error($"T6 Timeout: {T6 / 1000} sec.");
                    CommunicationStateChanging(ConnectionState.Retry);
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }
        }

        private static void ReleaseEncoderBuffer(IList<ArraySegment<byte>> e)
        {
            foreach (var b in e)
                ArrayPool<byte>.Shared.Return(b.Array);

            e.Clear();
            EncoderPool.Release(e);
        }

        private void HandleControlMessage(MessageHeader header)
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
                    _logger.Warning($"Received Unexpected Control Message: {header.MessageType.GetName()}");
                    return;
                }
            }
            _logger.Info($"Receive Control message: {header.MessageType.GetName()}");
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

            var bufferList = EncoderPool.Acquire();
            msg.EncodeTo(bufferList, EncodeHeader(ref header));

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

        private void SendDataMessageCompleteHandler(object socket, SocketAsyncEventArgs e)
        {
            ReleaseEncoderBuffer(e.BufferList);

            var completeToken = Unsafe.As<TaskCompletionSourceToken>(e.UserToken);

            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                CommunicationStateChanging(ConnectionState.Retry);
                return;
            }

            var messageSent = completeToken.MessageSent;
            _logger.MessageOut(messageSent, completeToken.Id);

            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T3))
                {
                    _logger.Error($"T3 Timeout[id=0x{completeToken.Id:X8}]: {T3 / 1000} sec.");
                    completeToken.SetException(new SecsException(completeToken.Id, Resources.T3Timeout));
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out completeToken);
            }

            if (completeToken.AutoDispose)
                messageSent.Dispose();
        }

        private void HandleDataMessage(ref MessageHeader header, SecsMessage msg)
        {
            var systembyte = header.SystemBytes;

            if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
            {
                _logger.MessageIn(msg, systembyte);
                _logger.Warning("Received Unrecognized Device Id Message");
                msg.Dispose();

                SendDataMessageAsync(new SecsMessage(9, 1, false, "Unrecognized Device Id", B(EncodeHeader(ref header))), NewSystemId);

                return;
            }

            if (msg.F % 2 != 0)
            {
                if (msg.S != 9)
                {
                    //Primary message
                    _logger.MessageIn(msg, systembyte);
                    PrimaryMessageReceived?.Invoke(this, new PrimaryMessageWrapper(this, header, msg));
                    msg.Dispose();
                    return;
                }
                // Error message systembyte
                unsafe
                {
                    var headerBytes = Unsafe.As<byte[]>(msg.SecsItem.Values);
                    Array.Reverse(headerBytes, 0, 10);
                    Unsafe.CopyBlock(
                        destination: Unsafe.AsPointer(ref systembyte),
                        source: Unsafe.AsPointer(ref headerBytes[0]),
                        byteCount: 4);
                }
            }

            // Secondary message
            _logger.MessageIn(msg, systembyte);
            TaskCompletionSourceToken ar;
            if (_replyExpectedMsgs.TryGetValue(systembyte, out ar))
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
#if !DISABLE_TIMER
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
            _replyExpectedMsgs.Clear();
            _stopImpl?.Invoke();

            if (_socket == null)
                return;

            if (_socket.Connected)
                _socket.Shutdown(SocketShutdown.Both);

            _socket.Dispose();
            _socket = null;
        }
        public void Start() => new TaskFactory(TaskScheduler.Default).StartNew(_startImpl);

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
            internal readonly int Id;
            internal readonly MessageType MsgType;
            internal readonly bool AutoDispose;

            internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg, int id, bool autoDispose, MessageType msgType = MessageType.DataMessage)
            {
                MessageSent = primaryMessageMsg;
                Id = id;
                MsgType = msgType;
                AutoDispose = autoDispose;
            }

            internal void HandleReplyMessage(SecsMessage replyMsg)
            {
                replyMsg.Name = MessageSent.Name;
                if (replyMsg.F == 0)
                {
                    SetException(new SecsException(Id, Resources.SxF0));
                    MessageSent.Dispose();
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
                    MessageSent.Dispose();
                    return;
                }

                SetResult(replyMsg);
            }
        }
    }
}