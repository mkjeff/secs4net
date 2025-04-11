using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.Options;
using PooledAwait;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace Secs4Net;

#if NET
[UnsupportedOSPlatform("browser")]
#endif
public sealed class HsmsConnection : ISecsConnection, IAsyncDisposable
{
    public event EventHandler<ConnectionState>? ConnectionChanged;
    public int T5 { get; }
    public int T6 { get; }
    public int T7 { get; }
    public int T8 { get; }
    public int LinkTestInterval { get; }
    public bool LinkTestEnabled
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

    public bool IsDisposed
        => Interlocked.CompareExchange(ref _disposeStage, DisposalComplete, DisposalComplete) == DisposalComplete;

    private readonly Func<CancellationToken, Task> _startImpl;
    private readonly Action _stopImpl;
    private readonly Timer _timer7;
    private readonly Timer _timer8;
    private readonly Timer _timerLinkTest;
    private readonly ConcurrentDictionary<int, ValueTaskCompletionSource<MessageType>> _replyExpectedMsgs = new();
    private readonly int _socketReceiveBufferSize;
#if !NET
    private readonly byte[] _socketReceiveBuffer;
#endif
    private readonly ISecsGemLogger _logger;
    private readonly PipeDecoder _pipeDecoder;
    private readonly Pipe _pipe;
    private readonly SemaphoreSlim _sendLock = new(initialCount: 1);

    private CancellationToken _stoppingToken;
    private CancellationTokenSource? _cancellationTokenSourceForPipeDecoder;
    private readonly CancellationTokenSource _cancellationSourceForControlMessageProcessing = new();

    public HsmsConnection(IOptions<SecsGemOptions> secsGemOptions, ISecsGemLogger logger)
    {
        var pipe = new Pipe(new PipeOptions(useSynchronizationContext: true));
        _pipeDecoder = new PipeDecoder(pipe.Reader, pipe.Writer);
        _pipe = pipe;
        _logger = logger;
        var options = secsGemOptions.Value;
        T5 = options.T5;
        T6 = options.T6;
        T7 = options.T7;
        T8 = options.T8;
        LinkTestInterval = options.LinkTestInterval;
        IpAddress = IPAddress.Parse(options.IpAddress);
        Port = options.Port;
        IsActive = options.IsActive;
        _socketReceiveBufferSize = options.SocketReceiveBufferSize;
#if !NET
        _socketReceiveBuffer = new byte[_socketReceiveBufferSize];
#endif

        Task.Run(() => HandleControlMessagesAsync(_cancellationSourceForControlMessageProcessing.Token), _cancellationSourceForControlMessageProcessing.Token);

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
#if !DISABLE_TIMER
            if (State == ConnectionState.Selected)
            {
                _ = SendLinkTestAsync();
            }

            async FireAndForget SendLinkTestAsync() => await SendControlMessage(MessageType.LinkTestRequest, MessageIdGenerator.NewId()).ConfigureAwait(false);
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
                        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            Blocking = false,
                            ReceiveBufferSize = _socketReceiveBufferSize,
                        };
#if NET
                        await socket.ConnectAsync(IpAddress, Port, cancellation).ConfigureAwait(false);
#else
                        await socket.ConnectAsync(IpAddress, Port).WithCancellation(cancellation).ConfigureAwait(false);
#endif

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

                await SendControlMessage(MessageType.SelectRequest, MessageIdGenerator.NewId(), cancellation).ConfigureAwait(false);
            };

            _stopImpl = delegate { };
        }
        else
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = false,
            };
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
#if NET
                        _socket = await server.AcceptAsync(cancellation).ConfigureAwait(false);
#else
                        _socket = await server.AcceptAsync().WithCancellation(cancellation).ConfigureAwait(false);
#endif
                        _socket.Blocking = false;
                        _socket.ReceiveBufferSize = _socketReceiveBufferSize;
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

    private void Disconnect()
    {
        _timer7.Change(Timeout.Infinite, Timeout.Infinite);
        _timer8.Change(Timeout.Infinite, Timeout.Infinite);
        StopPipeDecoder(ref _cancellationTokenSourceForPipeDecoder);
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

    public void Start(CancellationToken cancellation)
    {
        _stoppingToken = cancellation;
        Task.Run(() => _startImpl(cancellation), cancellation);
    }

    private async Task StartPipeDecoderConsumerAsync(CancellationToken cancellation)
    {
        try
        {
            await _pipeDecoder.StartAsync(cancellation).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            _logger.Error("Unexpected exception on StartAsyncStreamDecoderAsync", ex);
            Reconnect();
        }
    }


    private async Task StartPipeDecoderProducerAsync(CancellationToken cancellation)
    {
        var decoderInput = _pipeDecoder.Input;
        try
        {
            while (true)
            {
                Debug.Assert(_socket != null);
#if NET
                var memory = decoderInput.GetMemory(_socketReceiveBufferSize);
                var count = await _socket!.ReceiveAsync(memory, SocketFlags.None, cancellation).ConfigureAwait(false);
                decoderInput.Advance(count);
                await decoderInput.FlushAsync(cancellation).ConfigureAwait(false);
#else
                var count = await _socket!.ReceiveAsync(new ArraySegment<byte>(_socketReceiveBuffer), SocketFlags.None).WithCancellation(cancellation).ConfigureAwait(false);
                if (count > 0)
                {
                    await decoderInput.WriteAsync(_socketReceiveBuffer.AsMemory()[..count], cancellation).ConfigureAwait(false);
                }
#endif
                if (count == 0)
                {
                    Reconnect();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            _logger.Error("Unhandled exception occurred on PipeDecoder producer", ex);
            Reconnect();
        }
    }

    private void StopPipeDecoder(ref CancellationTokenSource? cancellationTokenSource)
    {
        if (cancellationTokenSource is { IsCancellationRequested: false })
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            _pipe.Reader.Complete();
            _pipe.Writer.Complete();
            _pipe.Reset();
        }
    }

    public void Reconnect()
        => CommunicationStateChanging(ConnectionState.Retry);

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
                _cancellationTokenSourceForPipeDecoder = new CancellationTokenSource();
                Task.Run(() => StartPipeDecoderConsumerAsync(_cancellationTokenSourceForPipeDecoder.Token));
                Task.Run(() => StartPipeDecoderProducerAsync(_cancellationTokenSourceForPipeDecoder.Token));
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
                Start(_stoppingToken);
                break;
        }
    }

    private async Task HandleControlMessagesAsync(CancellationToken cancellation)
    {
        try
        {
            await foreach (var item in _pipeDecoder.GetControlMessages(cancellation).WithCancellation(cancellation).ConfigureAwait(false))
            {
                await ProcessControlMessageAsync(item, cancellation).ConfigureAwait(continueOnCapturedContext: false);
            }
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested) { }
    }

    private async Task ProcessControlMessageAsync(MessageHeader header, CancellationToken cancellation)
    {
        try
        {
            if ((byte)header.MessageType % 2 == 0)
            {
                if (_replyExpectedMsgs.TryGetValue(header.Id, out var ar))
                {
                    ar.TrySetResult(header.MessageType);
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
                    await SendControlMessage(MessageType.SelectResponse, header.Id, cancellation).ConfigureAwait(false);
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
                    await SendControlMessage(MessageType.LinkTestResponse, header.Id, cancellation).ConfigureAwait(false);
                    break;
                case MessageType.SeparateRequest:
                    CommunicationStateChanging(ConnectionState.Retry);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Unhandled exception occurred when processing control message: " + header.ToString(), ex);
        }
    }

    private static readonly ReadOnlyMemory<byte> ControlMessageLengthBytes = new byte[] { 0, 0, 0, 10 };
    private async Task SendControlMessage(MessageType msgType, int id, CancellationToken cancellation = default)
    {
        var token = ValueTaskCompletionSource<MessageType>.Create();
        if ((byte)msgType % 2 == 1 && msgType != MessageType.SeparateRequest)
        {
            _replyExpectedMsgs[id] = token;
        }

        try
        {
            var buffer = EncodeControlMessage(msgType, id);
            await Unsafe.As<ISecsConnection>(this).SendAsync(buffer, cancellation).ConfigureAwait(false);

            _logger.Info("Sent Control Message: " + msgType);
            if (_replyExpectedMsgs.ContainsKey(id))
            {
#if NET
                await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T6), cancellation).ConfigureAwait(false);
#else
                if (await Task.WhenAny(token.Task, Task.Delay(T6, cancellation)).ConfigureAwait(false) != token.Task)
                {
                    _logger.Error($"T6 Timeout[id=0x{id:X8}]: {T6 / 1000} sec.");
                    CommunicationStateChanging(ConnectionState.Retry);
                }
#endif
            }
        }
#if NET
        catch (TimeoutException)
        {
            _logger.Error($"T6 Timeout[id=0x{id:X8}]: {T6 / 1000} sec.");
            CommunicationStateChanging(ConnectionState.Retry);
        }
#endif
        catch (Exception ex)
        {
            _logger.Error($"Unknown exception occurred when send control messages", ex);
            CommunicationStateChanging(ConnectionState.Retry);
        }
        finally
        {
            _replyExpectedMsgs.TryRemove(id, out _);
        }

        static ReadOnlyMemory<byte> EncodeControlMessage(MessageType msgType, int id)
        {
            var buffer = new MemoryBufferWriter<byte>(new byte[14]);
            buffer.Write(ControlMessageLengthBytes.Span);
            new MessageHeader
            {
                DeviceId = 0xFFFF,
                MessageType = msgType,
                Id = id
            }.EncodeTo(buffer);
            return buffer.WrittenMemory;
        }
    }

    private void StopT8Timer()
    {
        _logger.Debug($"Stop T8 Timer: {T8 / 1000} sec.");
        _timer8.Change(Timeout.Infinite, Timeout.Infinite);
    }

    private void StartT8Timer()
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
            await SendControlMessage(MessageType.SeparateRequest, MessageIdGenerator.NewId()).ConfigureAwait(false);
        }

        Disconnect();
        _cancellationSourceForControlMessageProcessing.Cancel();
        _cancellationSourceForControlMessageProcessing.Dispose();
        _timer7.Dispose();
        _timer8.Dispose();
        _timerLinkTest.Dispose();
    }

    Task ISecsConnection.SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellation)
        => SendAsync(buffer, cancellation);

#if NET
    private async Task SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellation)
    {
        await _sendLock.WaitAsync(cancellation).ConfigureAwait(false);
        try
        {
            do
            {
                Debug.Assert(_socket != null);
                var length = await _socket.SendAsync(buffer, SocketFlags.None, cancellation).ConfigureAwait(false);
                Debug.WriteLine($"Socket sent {length} bytes.");
                buffer = buffer[length..];
            } while (!buffer.IsEmpty);
        }
        finally
        {
            _sendLock.Release();
        }
    }
#else
    private async Task SendAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellation)
    {
        if (!System.Runtime.InteropServices.MemoryMarshal.TryGetArray(buffer, out var arr))
        {
            throw new InvalidOperationException();
        }
        await _sendLock.WaitAsync(cancellation).ConfigureAwait(false);
        try
        {
            do
            {
                Debug.Assert(_socket != null);
                var length = await _socket.SendAsync(arr, SocketFlags.None).WithCancellation(cancellation).ConfigureAwait(false);
                arr = new ArraySegment<byte>(arr.Array, arr.Offset + length, arr.Count - length);
                Debug.WriteLine($"Socket sent {length} bytes.");
            } while (arr.Count > 0);
        }
        finally
        {
            _sendLock.Release();
        }
    }
#endif

    IAsyncEnumerable<(MessageHeader header, Item? rootItem)> ISecsConnection.GetDataMessages(CancellationToken cancellation)
        => _pipeDecoder.GetDataMessages(cancellation);
}
