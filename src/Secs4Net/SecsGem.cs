using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.Options;
using PooledAwait;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Secs4Net;

public interface ISecsGem
{
    ushort DeviceId { get; }

    /// <summary>
    /// Get primary messages from async-stream
    /// </summary>
    /// <param name="cancellation"></param>
    IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Send a message to device asynchronously and get reply message.
    /// </summary>
    /// <param name="message">primary message</param>
    /// <returns>Secondary message, or null if <paramref name="message" />'s <see cref="SecsMessage.ReplyExpected"/> is <see langword="false" /> </returns>
    Task<SecsMessage> SendAsync(SecsMessage message, CancellationToken cancellation = default);
}

public sealed class SecsGem : ISecsGem, IDisposable
{
    private const int DisposalNotStarted = 0;
    private const int DisposalComplete = 1;
    private int _disposeStage;
    private readonly ISecsGemLogger _logger;
    private readonly ISecsConnection _hsmsConnector;

    public ushort DeviceId { get; }
    public int T3 { get; }

    private readonly Channel<PrimaryMessageWrapper> _primaryMessageChannel = Channel
        .CreateUnbounded<PrimaryMessageWrapper>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = true,
            AllowSynchronousContinuations = false,
        });

    private readonly ConcurrentDictionary<int, (string? messageName, ValueTaskCompletionSource<SecsMessage> completeSource)> _replyExpectedMessages = new();
    private readonly CancellationTokenSource _cancellationSourceForDataMessageProcessing = new();
    private int _recentlyMaxEncodedByteLength;

    public SecsGem(IOptions<SecsGemOptions> secsGemOptions, ISecsConnection hsmsConnector, ISecsGemLogger logger)
    {
        var options = secsGemOptions.Value;
        DeviceId = options.DeviceId;
        T3 = options.T3;
        _recentlyMaxEncodedByteLength = options.EncodeBufferInitialSize;

        _hsmsConnector = hsmsConnector;
        _logger = logger;

        Task.Run(async () =>
        {
            var cancellationToken = _cancellationSourceForDataMessageProcessing.Token;
            await foreach (var (header, rootItem) in _hsmsConnector.GetDataMessages(cancellationToken).WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                await ProcessDataMessageAsync(header, rootItem, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
        });
    }

    internal async Task<SecsMessage> SendDataMessageAsync(SecsMessage message, int id, CancellationToken cancellation)
    {
        if (_hsmsConnector.State != ConnectionState.Selected)
        {
            throw new SecsException("Device is not selected");
        }

        var token = ValueTaskCompletionSource<SecsMessage>.Create();
        if (message.ReplyExpected)
        {
            _replyExpectedMessages[id] = (message.Name, token);
        }

        try
        {
            using (var buffer = new ArrayPoolBufferWriter<byte>(initialCapacity: _recentlyMaxEncodedByteLength))
            {
                EncodeMessage(message, id, DeviceId, buffer);
                await _hsmsConnector.SendAsync(buffer.WrittenMemory, cancellation).ConfigureAwait(false);

                if (buffer.WrittenCount > _recentlyMaxEncodedByteLength)
                {
                    _recentlyMaxEncodedByteLength = buffer.WrittenCount;
                }
            }

            _logger.MessageOut(message, id);

            if (!message.ReplyExpected)
            {
                return null!;
            }

#if NET
            return await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T3), cancellation).ConfigureAwait(false);
#else
            if (await Task.WhenAny(token.Task, Task.Delay(T3, cancellation)).ConfigureAwait(false) != token.Task)
            {
                throw new SecsException(message, Resources.T3Timeout);
            }
            return token.Task.Result;
#endif
        }
        catch (SocketException)
        {
            _hsmsConnector.Reconnect();
            throw;
        }
#if NET
        catch (TimeoutException)
        {
            _logger.Error($"T3 Timeout[id=0x{id:X8}]: {T3 / 1000} sec.");
            throw new SecsException(message, Resources.T3Timeout);
        }
#endif
        finally
        {
            _replyExpectedMessages.TryRemove(id, out _);
        }
    }

    public Task<SecsMessage> SendAsync(SecsMessage message, CancellationToken cancellation = default)
        => SendDataMessageAsync(message, MessageIdGenerator.NewId(), cancellation);

    public IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default)
        => _primaryMessageChannel.Reader.ReadAllAsync(cancellation);

    private async Task ProcessDataMessageAsync(MessageHeader header, Item? rootItem, CancellationToken cancellation)
    {
        var msg = new SecsMessage(header.S, header.F, header.ReplyExpected)
        {
            SecsItem = rootItem,
        };

        try
        {
            if (header.DeviceId != DeviceId && header.S != 9 && header.F != 1)
            {
                _logger.MessageIn(msg, header.Id);
                _logger.Warning("Received Unrecognized Device Id Message");
                var headerBytes = new byte[10];
                header.EncodeTo(new MemoryBufferWriter<byte>(headerBytes));
                var s9f1 = new SecsMessage(9, 1, replyExpected: false)
                {
                    Name = "Unrecognized Device Id",
                    SecsItem = Item.B(headerBytes),
                };
                await SendDataMessageAsync(s9f1, MessageIdGenerator.NewId(), cancellation).ConfigureAwait(false);
                return;
            }

            var id = header.Id;
            if (header.F % 2 != 0)
            {
                if (header.S != 9)
                {
                    //Primary message
                    _logger.MessageIn(msg, header.Id);
                    await _primaryMessageChannel.Writer.WriteAsync(new PrimaryMessageWrapper(this, msg, id), cancellation).ConfigureAwait(false);
                    return;
                }
                // Error message
                if (rootItem is { Format: not (SecsFormat.List or SecsFormat.ASCII or SecsFormat.JIS8) } dataItem
                    && dataItem.GetMemory<byte>() is { Length: >= 10 } headerBytes)
                {
                    id = BinaryPrimitives.ReadInt32BigEndian(headerBytes.Span.Slice(6, 4));
                }
                else
                {
                    _logger.Warning("Received S9Fy message without primary message's header bytes.");
                }
            }

            // Secondary message
            _logger.MessageIn(msg, id);
            if (_replyExpectedMessages.TryGetValue(id, out var token))
            {
                msg.Name = token.messageName;
                HandleReplyMessage(token.completeSource, msg);
            }
            else
            {
                _logger.Warning($"Received unexpected secondary message[0x{id:X8}]. Maybe T3 timeout.");
            }
        }
        catch (Exception ex)
        {
            if (cancellation.IsCancellationRequested)
            {
                return;
            }
            _logger.Error("Unhandled exception occurred when processing data message", msg, ex);
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
        {
            throw new ObjectDisposedException(nameof(SecsGem));
        }

        _cancellationSourceForDataMessageProcessing.Cancel();
        _cancellationSourceForDataMessageProcessing.Dispose();
        _replyExpectedMessages.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET 
    public static void EncodeMessage(SecsMessage msg, int id, ushort deviceId, ArrayPoolBufferWriter<byte> buffer)
#else
    public static unsafe void EncodeMessage(SecsMessage msg, int id, ushort deviceId, ArrayPoolBufferWriter<byte> buffer)
#endif
    {
        buffer.GetSpan(14);
        // reserve 4 byte for total length
        buffer.Advance(sizeof(int));
        new MessageHeader
        {
            DeviceId = deviceId,
            ReplyExpected = msg.ReplyExpected,
            S = msg.S,
            F = msg.F,
            MessageType = MessageType.DataMessage,
            Id = id
        }.EncodeTo(buffer);
        msg.SecsItem?.EncodeTo(buffer);

#if NET
        var lengthBytes = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(buffer.WrittenSpan), 4);
#else
        var lengthBytes = new Span<byte>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(buffer.WrittenSpan)), 4);
#endif
        BinaryPrimitives.WriteInt32BigEndian(lengthBytes, buffer.WrittenCount - sizeof(int));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void HandleReplyMessage(ValueTaskCompletionSource<SecsMessage> source, SecsMessage secondaryMessage)
    {
        if (secondaryMessage.F == 0)
        {
            source.TrySetException(new SecsException(secondaryMessage, Resources.SxF0));
            return;
        }

        if (secondaryMessage.S == 9)
        {
            switch (secondaryMessage.F)
            {
                case 1:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F1));
                    break;
                case 3:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F3));
                    break;
                case 5:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F5));
                    break;
                case 7:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F7));
                    break;
                case 9:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F9));
                    break;
                case 11:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F11));
                    break;
                case 13:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9F13));
                    break;
                default:
                    source.TrySetException(new SecsException(secondaryMessage, Resources.S9Fy));
                    break;
            }
            return;
        }

        source.TrySetResult(secondaryMessage);
    }
}
