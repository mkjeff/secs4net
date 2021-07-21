using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class HsmsConnection : BackgroundService, IHsmsConnection, IAsyncDisposable
    {
        public event EventHandler<ConnectionState>? ConnectionChanged;
        public int T5 { get; }
        public int T6 { get; }
        public int T7 { get; }
        public int T8 { get; }
        public int LinkTestInterval { get; }
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

        public ConnectionState State { get; private set; }
        public bool IsActive { get; }
        public IPAddress IpAddress { get; }
        public int Port { get; }

        public string DeviceIpAddress
            => IsActive
            ? IpAddress.ToString()
            : ((IPEndPoint?)_socket?.RemoteEndPoint)?.Address?.ToString() ?? "NA";

        private Socket? _socket;
        private const int DisposalNotStarted = 0;
        private const int DisposalComplete = 1;
        private int _disposeStage;
        private readonly ISecsGemLogger _logger;

        public bool IsDisposed
            => Interlocked.CompareExchange(ref _disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;

        private readonly Func<CancellationToken, Task> _startImpl;
        private readonly Action _stopImpl;
        private readonly Timer _timer7;
        private readonly Timer _timer8;
        private readonly Timer _timerLinkTest;
        private readonly ConcurrentDictionary<int, TaskCompletionSource> _replyExpectedMsgs = new();
        private readonly Memory<byte> _socketReceiveBuffer;

        public PipeDecoder PipeDecoder { get; }
        private CancellationToken _stoppingToken;

        public HsmsConnection(IOptions<SecsGemOptions> secsGemOptions, ISecsGemLogger logger)
        {
            var pipe = new Pipe();
            PipeDecoder = new PipeDecoder(pipe.Reader, pipe.Writer);
            _logger = logger;
            var options = secsGemOptions.Value;
            T5 = options.T5;
            T6 = options.T6;
            T7 = options.T7;
            T8 = options.T8;
            LinkTestInterval = options.LinkTestInterval;
            IpAddress = options.IpAddress ?? throw new ArgumentNullException(nameof(options.IpAddress));
            Port = options.Port;
            IsActive = options.IsActive;
            _socketReceiveBuffer = new byte[options.SocketReceiveBufferSize];

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
                    await SendControlMessage(MessageType.LinkTestRequest, SystemByteGenerator.New()).ConfigureAwait(false);
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
                            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            await socket.ConnectAsync(IpAddress, Port, cancellation).ConfigureAwait(false);
                            _socket = socket;
                            CommunicationStateChanging(ConnectionState.Connected);
                            connected = true;
                        }
                        catch (Exception ex) when (!IsDisposed)
                        {
                            _logger.Error(ex.Message);
                            _logger.Info($"Start T5 Timer: {T5 / 1000} sec.");
                            await Task.Delay(T5, cancellation).ConfigureAwait(false);
                        }
                    } while (!connected);

                    await SendControlMessage(MessageType.SelectRequest, SystemByteGenerator.New(), cancellation).ConfigureAwait(false);
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
                            CommunicationStateChanging(ConnectionState.Connected);
                            connected = true;
                        }
                        catch (Exception ex) when (!IsDisposed)
                        {
                            _logger.Error(ex.Message);
                            await Task.Delay(2000, cancellation).ConfigureAwait(false);
                        }
                    } while (!connected);
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

        private async Task StartDecoderAsync(CancellationToken cancellation)
        {
            try
            {
                await PipeDecoder.StartAsync(cancellation).ConfigureAwait(false);
            }
            catch (Exception ex) when (!cancellation.IsCancellationRequested)
            {
                _logger.Error("Unexpected exception on StartAsyncStreamDecoderAsync", ex);
                Reconnect();
            }
        }

        private void Disconnect()
        {
            _timer7.Change(Timeout.Infinite, Timeout.Infinite);
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
            _timerLinkTest.Change(Timeout.Infinite, Timeout.Infinite);
            TryStopSocketReceiveLoop();
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            _ = AsyncHelper.LongRunningAsync(() =>
                HandleControlMessagesAsync(_stoppingToken), _stoppingToken);

            _ = AsyncHelper.LongRunningAsync(() =>
                StartDecoderAsync(_stoppingToken), _stoppingToken);

            _ = AsyncHelper.LongRunningAsync(() => _startImpl(stoppingToken), stoppingToken);
            return Task.CompletedTask;
        }

        public void Reconnect()
            => CommunicationStateChanging(ConnectionState.Retry);

        private CancellationTokenSource? _cancellationTokenSourceForSocketReceiveLoop;
        private void CommunicationStateChanging(ConnectionState newState)
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
                    TryStopSocketReceiveLoop();
                    _cancellationTokenSourceForSocketReceiveLoop = new CancellationTokenSource();
                    _ = AsyncHelper.LongRunningAsync(() =>
                        StartScoketReceiveLoopAsync(_cancellationTokenSourceForSocketReceiveLoop.Token), _cancellationTokenSourceForSocketReceiveLoop.Token);
                    _logger.Info($"Start T7 Timer: {T7 / 1000} sec.");
                    _timer7.Change(T7, Timeout.Infinite);
#endif
                    break;
                case ConnectionState.Retry:
                    if (IsDisposed)
                    {
                        return;
                    }

                    Disconnect();
                    Task.Run(() => _startImpl(_stoppingToken));
                    break;
            }
        }

        private void TryStopSocketReceiveLoop()
        {
            if (_cancellationTokenSourceForSocketReceiveLoop is not null && !_cancellationTokenSourceForSocketReceiveLoop.IsCancellationRequested)
            {
                _cancellationTokenSourceForSocketReceiveLoop.Cancel();
                _cancellationTokenSourceForSocketReceiveLoop.Dispose();
                _cancellationTokenSourceForSocketReceiveLoop = null;
            }
        }

        private Task HandleControlMessagesAsync(CancellationToken cancellation)
            => PipeDecoder.GetControlMessages(cancellation)
            .ForEachAwaitWithCancellationAsync(async (header, ct) =>
            {
                var systembyte = header.SystemBytes;
                if ((byte)header.MessageType % 2 == 0)
                {
                    if (_replyExpectedMsgs.TryGetValue(systembyte, out var ar))
                    {
                        ar.TrySetResult();
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

        private static readonly ReadOnlyMemory<byte> ControlMessageLengthBytes = new byte[] { 0, 0, 0, 10 };
        private async Task SendControlMessage(MessageType msgType, int systembyte, CancellationToken cancellation = default)
        {
            var token = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            if ((byte)msgType % 2 == 1 && msgType != MessageType.SeperateRequest)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            try
            {
                var buffer = EncodeControlMessage(msgType, systembyte);
                await SendAsync(this, buffer, cancellation).ConfigureAwait(false);

                _logger.Info("Sent Control Message: " + msgType);
                if (_replyExpectedMsgs.ContainsKey(systembyte))
                {
#if DISABLE_TIMER
                    await token.Task.WaitAsync(cancellation).ConfigureAwait(false);
#else
                    await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T6), cancellation).ConfigureAwait(false);
#endif
                }
            }
            catch (TimeoutException)
            {
                _logger.Error($"T6 Timeout[id=0x{systembyte:X8}]: {T6 / 1000} sec.");
                CommunicationStateChanging(ConnectionState.Retry);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unknown exception occurred when send control messages", ex);
                CommunicationStateChanging(ConnectionState.Retry);
            }
            finally
            {
                _replyExpectedMsgs.TryRemove(systembyte, out _);
            }

            static ReadOnlyMemory<byte> EncodeControlMessage(MessageType msgType, int systembyte)
            {
                var buffer = new MemoryBufferWriter<byte>(new byte[14]);
                buffer.Write(ControlMessageLengthBytes.Span);
                new MessageHeader(
                    deviceId: 0xFFFF,
                    messageType: msgType,
                    systemBytes: systembyte).EncodeTo(buffer);
                return buffer.WrittenMemory;
            }
        }

        void StopT8Timer()
        {
            _logger.Debug($"Stop T8 Timer: {T8 / 1000} sec.");
            _timer8.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void StartT8Timer()
        {
            _logger.Debug($"Start T8 Timer: {T8 / 1000} sec.");
            _timer8.Change(T8, Timeout.Infinite);
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
            {
                return;
            }

            ConnectionChanged = null;
            if (State == ConnectionState.Selected)
            {
                await SendControlMessage(MessageType.SeperateRequest, SystemByteGenerator.New()).ConfigureAwait(false);
            }

            Disconnect();
            _timer7.Dispose();
            _timer8.Dispose();
            _timerLinkTest.Dispose();
        }

        private async Task StartScoketReceiveLoopAsync(CancellationToken cancellationToken)
        {
            var writer = PipeDecoder.Input;
            while (true)
            {
                Debug.Assert(_socket != null);
                var count = await _socket.ReceiveAsync(_socketReceiveBuffer, SocketFlags.None, cancellationToken).ConfigureAwait(false);
                await writer.WriteAsync(_socketReceiveBuffer.Slice(0, count), cancellationToken).ConfigureAwait(false);
            }
        }

        public ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            Debug.Assert(_socket != null);
            return _socket.SendAsync(buffer, SocketFlags.None, cancellationToken);
        }

        internal static async ValueTask SendAsync(IHsmsConnection connector, ReadOnlyMemory<byte> bytesToTransfer, CancellationToken cancellation)
        {
            do
            {
                var length = await connector.SendAsync(bytesToTransfer, cancellation).ConfigureAwait(false);
                bytesToTransfer = bytesToTransfer.Slice(length);
            } while (!bytesToTransfer.IsEmpty);
        }
    }
}
