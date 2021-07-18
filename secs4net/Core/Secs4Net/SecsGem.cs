using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface ISecsGem : IAsyncDisposable
    {
        int T8 { get; set; }
        bool LinkTestEnable { get; set; }
        IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default);
        Task<SecsMessage> SendAsync(SecsMessage msg, CancellationToken cancellation = default);
        void Start();
        internal void StopT8Timer();
        internal void StartT8Timer();
        event EventHandler<ConnectionState>? ConnectionChanged;
    }

    public sealed class SecsGem : ISecsGem
    {
        public event EventHandler<ConnectionState>? ConnectionChanged;

        public ISecsGemLogger Logger
        {
            get => _logger;
            set => _logger = value ?? DefaultLogger;
        }
        private ISecsGemLogger _logger = DefaultLogger;

        /// <summary>
        /// Connection state
        /// </summary>
        public ConnectionState State { get; private set; }

        public ushort DeviceId { get; set; } = 0;
        public int T3 { get; set; } = 45000;
        public int T5 { get; set; } = 10000;
        public int T6 { get; set; } = 5000;
        public int T7 { get; set; } = 10000;
        public int T8 { get; set; } = 5000;

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
        private bool _linkTestEnable;

        public bool IsActive { get; }
        public IPAddress IpAddress { get; }
        public int Port { get; }
        public int DecoderBufferSize { get; private set; }

        private const int DisposalNotStarted = 0;
        private const int DisposalComplete = 1;
        private int _disposeStage;

        public bool IsDisposed
            => Interlocked.CompareExchange(ref _disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;

        /// <summary>
        /// remote device endpoint address
        /// </summary>
        public string DeviceIpAddress
            => IsActive
            ? IpAddress.ToString()
            : ((IPEndPoint?)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        private Socket? _socket;

        private readonly AsyncStreamDecoder _decoder;
        private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new();
        private readonly Timer _timer7;
        private readonly Timer _timer8;
        private readonly Timer _timerLinkTest;

        private readonly Func<CancellationToken, Task> _startImpl;
        private readonly Action _stopImpl;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static readonly SecsMessage ControlMessage = new(0, 0);
        private static readonly byte[] ControlMessageLengthBytes = new byte[] { 0, 0, 0, 10 };
        private static readonly DefaultSecsGemLogger DefaultLogger = new();

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

            _decoder = new AsyncStreamDecoder(receiveBufferSize, this);
            IpAddress = ip;
            Port = port;
            IsActive = isActive;
            DecoderBufferSize = receiveBufferSize;

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

            _timerLinkTest = new Timer(async delegate
            {
#if !DISABLE_TIMER
                if (State == ConnectionState.Selected)
                {
                    using var cts = new CancellationTokenSource(10000);
                    await SendControlMessage(MessageType.LinkTestRequest, SystemByteGenerator.New(), cts.Token).ConfigureAwait(false);
                }
#endif
            }, null, Timeout.Infinite, Timeout.Infinite);

            if (IsActive)
            {
                _startImpl = async cancellation =>
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
                            await _socket.ConnectAsync(IpAddress, Port, cancellation).ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex) when (!IsDisposed)
                        {
                            _logger.Error(ex.Message);
                            _logger.Info($"Start T5 Timer: {T5 / 1000} sec.");
                            await Task.Delay(T5, cancellation).ConfigureAwait(false);
                        }
                    } while (!connected);

                    await Task.WhenAll(
                        SendControlMessage(MessageType.SelectRequest, SystemByteGenerator.New(), cancellation),
                        StartAsyncStreamDecoderAsync(cancellation)).ConfigureAwait(false);
                };

                _stopImpl = delegate { };
            }
            else
            {
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IpAddress, Port));
                server.Listen(0);

                _startImpl = async cancellation =>
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
                            _socket = await server.AcceptAsync(cancellation).ConfigureAwait(false);
                            connected = true;
                        }
                        catch (Exception ex)
                        {
                            if (IsDisposed)
                            {
                                return;
                            }

                            _logger.Error(ex.Message);
                            await Task.Delay(2000, cancellation).ConfigureAwait(false);
                        }
                    } while (!connected);

                    await StartAsyncStreamDecoderAsync(cancellation).ConfigureAwait(false);
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

        private async Task StartAsyncStreamDecoderAsync(CancellationToken cancellation)
        {
            try
            {
                CommunicationStateChanging(ConnectionState.Connected);
                Debug.Assert(_socket != null);
                await _decoder.StartReceivedAsync(new SocketDecoderSource(_socket), cancellation).ConfigureAwait(false);
            }
            catch (SocketException ex)
            {
                _logger.Error($"Socket error occurred on StartSocketReceive:{ex.Message}, ErrorCode:{ex.SocketErrorCode}", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
            catch (Exception ex) when (!cancellation.IsCancellationRequested && _socket is not null && !IsDisposed)
            {
                _logger.Error("Unexpected exception on StartSocketReceive", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
        }

        private Task StartControlMessageConsumerAsync(CancellationToken cancellation)
            => _decoder.GetControlMessages(cancellation)
            .ForEachAwaitWithCancellationAsync(async (header, ct) =>
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
                        await SendControlMessage(MessageType.SelectResponse, systembyte, ct).ConfigureAwait(false);
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
                        await SendControlMessage(MessageType.LinkTestResponse, systembyte, ct).ConfigureAwait(false);
                        break;
                    case MessageType.SeperateRequest:
                        CommunicationStateChanging(ConnectionState.Retry);
                        break;
                }
            }, cancellation);

        void ISecsGem.StopT8Timer()
        {
            _logger.Debug($"Stop T8 Timer: {T8 / 1000} sec.");
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void ISecsGem.StartT8Timer()
        {
            _logger.Debug($"Start T8 Timer: {T8 / 1000} sec.");
            _timer8.Change(T8, Timeout.Infinite);
        }

        private async Task SendControlMessage(MessageType msgType, int systembyte, CancellationToken cancellation)
        {
            Debug.Assert(_socket != null);
            var token = new TaskCompletionSourceToken(ControlMessage, systembyte, msgType);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            try
            {
                using (var buffer = new ArrayPoolBufferWriter<byte>(initialCapacity: 14))
                {
                    buffer.Write(ControlMessageLengthBytes);
                    new MessageHeader(
                        deviceId: 0xFFFF,
                        messageType: msgType,
                        systemBytes: systembyte).EncodeTo(buffer);

                    await SendAsync(buffer.WrittenMemory, cancellation).ConfigureAwait(false);
                }

                _logger.Info("Sent Control Message: " + msgType);
                if (_replyExpectedMsgs.ContainsKey(systembyte))
                {
#if DISABLE_TIMER
                    await token.Task.WaitAsync(cancellation).ConfigureAwait(false);
#else
                    await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T6)).ConfigureAwait(false);
#endif
                }
            }
            catch (SocketException)
            {
                CommunicationStateChanging(ConnectionState.Retry);
                throw;
            }
            catch (TimeoutException)
            {
                _logger.Error($"T6 Timeout[id=0x{systembyte:X8}]: {T6 / 1000} sec.");
                CommunicationStateChanging(ConnectionState.Retry);
            }
            finally
            {
                _replyExpectedMsgs.TryRemove(systembyte, out _);
            }
        }

        internal async Task<SecsMessage> SendDataMessageAsync(SecsMessage msg, int systembyte, CancellationToken cancellation)
        {
            if (State != ConnectionState.Selected)
            {
                throw new SecsException("Device is not selected");
            }

            Debug.Assert(_socket != null);
            var token = new TaskCompletionSourceToken(msg, systembyte, MessageType.DataMessage);
            if (msg.ReplyExpected)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            try
            {
                using (var buffer = new ArrayPoolBufferWriter<byte>(initialCapacity: 14))
                {
                    EncodeMessage(msg, systembyte, DeviceId, buffer);
                    await SendAsync(buffer.WrittenMemory, cancellation).ConfigureAwait(false);
                }

                _logger.MessageOut(msg, token.Id);

                if (!msg.ReplyExpected)
                {
                    return default!;
                }

                return await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T3), cancellation).ConfigureAwait(false);
            }
            catch (SocketException)
            {
                CommunicationStateChanging(ConnectionState.Retry);
                throw;
            }
            catch (TimeoutException)
            {
                _logger.Error($"T3 Timeout[id=0x{systembyte:X8}]: {T3 / 1000} sec.");
                throw new SecsException(msg, Resources.T3Timeout);
            }
            finally
            {
                _replyExpectedMsgs.TryRemove(systembyte, out _);
            }
        }

        internal static void EncodeMessage(SecsMessage msg, int systembyte, ushort deviceId, ArrayPoolBufferWriter<byte> buffer)
        {
            // reserve 4 byte for total length
            var lengthSpan = buffer.GetSpan(sizeof(int)).Slice(0, sizeof(int));
            buffer.Advance(sizeof(int));

            msg.EncodeTo(buffer, deviceId, systembyte);

            BitConverter.TryWriteBytes(lengthSpan, buffer.WrittenCount - sizeof(int));
            lengthSpan.Reverse();
        }

        private async ValueTask SendAsync(ReadOnlyMemory<byte> bytesToTransfer, CancellationToken cancellation)
        {
            do
            {
                Debug.Assert(_socket != null);
                var length = await _socket.SendAsync(bytesToTransfer, SocketFlags.None, cancellation).ConfigureAwait(false);
                bytesToTransfer = bytesToTransfer.Slice(length);
            } while (!bytesToTransfer.IsEmpty);
        }

        internal void CommunicationStateChanging(ConnectionState newState)
        {
            State = newState;
            ConnectionChanged?.Invoke(this, State);

            switch (State)
            {
                case ConnectionState.Selected:
#if !DISABLE_TIMER
                    _timer7.Change(Timeout.Infinite, Timeout.Infinite);
                    _logger.Info("Stop T7 Timer");
#endif
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
                    Start();
                    break;
            }
        }

        private void Reset()
        {
            _timer7.Change(Timeout.Infinite, Timeout.Infinite);
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
            _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
            _replyExpectedMsgs.Clear();
            _stopImpl.Invoke();

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

        public void Start()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => StartControlMessageConsumerAsync(_cancellationTokenSource.Token), TaskCreationOptions.LongRunning).Unwrap();
            Task.Factory.StartNew(() => _startImpl(_cancellationTokenSource.Token), TaskCreationOptions.LongRunning).Unwrap();
        }

        /// <summary>
        /// Asynchronously send message to device .
        /// </summary>
        /// <param name="msg">primary message</param>
        /// <returns>secondary message</returns>
        public Task<SecsMessage> SendAsync(SecsMessage msg, CancellationToken cancellation = default)
            => SendDataMessageAsync(msg, SystemByteGenerator.New(), cancellation);

        public async IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync([EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await foreach (var (header, msg) in _decoder.GetDataMessages(cancellation))
            {
                var systembyte = header.SystemBytes;

                if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
                {
                    _logger.MessageIn(msg, systembyte);
                    _logger.Warning("Received Unrecognized Device Id Message");
                    var headerBytes = new byte[10];
                    header.EncodeTo(new MemoryBufferWriter<byte>(headerBytes));
                    var s9f1 = new SecsMessage(9, 1, replyExpected: false)
                    {
                        Name = "Unrecognized Device Id",
                        SecsItem = Item.B(headerBytes),
                    };
                    await SendDataMessageAsync(s9f1, SystemByteGenerator.New(), cancellation).ConfigureAwait(false);
                    continue;
                }

                if (msg.F % 2 != 0)
                {
                    if (msg.S != 9)
                    {
                        //Primary message
                        _logger.MessageIn(msg, systembyte);
                        yield return new PrimaryMessageWrapper(this, header, msg);
                        continue;
                    }
                    // Error message
                    if (msg.SecsItem?.GetValues<byte>() is not { Length: 10 } headerBytes)
                    {
                        _logger.Warning("Can't get expected header bytes");
                    }
                    else
                    {
                        var systemBytes = headerBytes.AsSpan().Slice(6, 4).ToArray();
                        Array.Reverse(systemBytes);
                        systembyte = BitConverter.ToInt32(systemBytes);
                    }
                }

                // Secondary message
                _logger.MessageIn(msg, systembyte);
                if (_replyExpectedMsgs.TryGetValue(systembyte, out var token))
                {
                    token.HandleReplyMessage(msg);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            ConnectionChanged = null;
            if (State == ConnectionState.Selected)
            {
                await SendControlMessage(MessageType.SeperateRequest, SystemByteGenerator.New(), CancellationToken.None).ConfigureAwait(false);
            }

            Reset();
            _timer7.Dispose();
            _timer8.Dispose();
            _timerLinkTest.Dispose();
        }

        private sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
        {
            public SecsMessage MessageSent { get; }
            public int Id { get; }
            public MessageType MessageType { get; }

            internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg, int id, MessageType messageType)
                : base(TaskCreationOptions.RunContinuationsAsynchronously)
            {
                MessageSent = primaryMessageMsg;
                Id = id;
                MessageType = messageType;
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
    }
}
