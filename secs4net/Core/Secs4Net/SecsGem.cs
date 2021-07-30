using Microsoft.Extensions.Options;
using Microsoft.Toolkit.HighPerformance.Buffers;
using PooledAwait;
using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface ISecsGem
    {
        /// <summary>
        /// Get primary messages from async-stream
        /// </summary>
        /// <param name="cancellation"></param>
        IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Send a message to device asynchronously and get reply message.
        /// </summary>
        /// <param name="message">primary message</param>
        /// <returns>Secondary message, or null if <paramref name="message" />'s <see cref="SecsMessage.ReplyExpected">ReplyExpected</see> is <see langword="false" /> </returns>
        ValueTask<SecsMessage> SendAsync(SecsMessage message, CancellationToken cancellation = default);
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
            .CreateBounded<PrimaryMessageWrapper>(new BoundedChannelOptions(capacity: 16)
            {
                SingleReader = false,
                SingleWriter = true,
                AllowSynchronousContinuations = false,
                FullMode = BoundedChannelFullMode.Wait,
            });

        private readonly ConcurrentDictionary<int, (SecsMessage primary, ValueTaskCompletionSource<SecsMessage> completeSource)> _replyExpectedMsgs = new();
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

            _ = AsyncHelper.LongRunningAsync(() =>
                _hsmsConnector.GetDataMessages(_cancellationSourceForDataMessageProcessing.Token)
                    .ForEachAwaitWithCancellationAsync(ProcessDataMessageAsync, _cancellationSourceForDataMessageProcessing.Token));
        }

        internal async PooledValueTask<SecsMessage> SendDataMessageAsync(SecsMessage message, int id, CancellationToken cancellation)
        {
            if (_hsmsConnector.State != ConnectionState.Selected)
            {
                throw new SecsException("Device is not selected");
            }

            var token = ValueTaskCompletionSource<SecsMessage>.Create();
            if (message.ReplyExpected)
            {
                _replyExpectedMsgs[id] = (message, token);
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
                if (await Task.WhenAny(token.Task, Task.Delay(T3, cancellation)) != token.Task)
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
                _replyExpectedMsgs.TryRemove(id, out _);
            }
        }

        internal static void EncodeMessage(SecsMessage msg, int id, ushort deviceId, ArrayPoolBufferWriter<byte> buffer)
        {
            // reserve 4 byte for total length
            var lengthBytes = buffer.GetSpan(sizeof(int)).Slice(0, sizeof(int));
            buffer.Advance(sizeof(int));
            new MessageHeader(
                deviceId,
                msg.ReplyExpected,
                msg.S,
                msg.F,
                MessageType.DataMessage,
                id).EncodeTo(buffer);
            msg.SecsItem?.EncodeTo(buffer);

            BinaryPrimitives.WriteInt32BigEndian(lengthBytes, buffer.WrittenCount - sizeof(int));
        }

        public ValueTask<SecsMessage> SendAsync(SecsMessage message, CancellationToken cancellation = default)
            => SendDataMessageAsync(message, MessageIdGenerator.NewId(), cancellation);

        public IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default)
            => _primaryMessageChannel.Reader.ReadAllAsync(cancellation);

        private async Task ProcessDataMessageAsync((MessageHeader header, Item? rootItem) data, CancellationToken cancellation)
        {
            var (header, rootItem) = data;
            var msg = new SecsMessage(header.S, header.F, header.ReplyExpected)
            {
                SecsItem = rootItem,
            };

            try
            {
                if (header.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
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
                if (msg.F % 2 != 0)
                {
                    if (msg.S != 9)
                    {
                        //Primary message
                        _logger.MessageIn(msg, header.Id);
                        await _primaryMessageChannel.Writer.WriteAsync(new PrimaryMessageWrapper(this, msg, id), cancellation).ConfigureAwait(false);
                        return;
                    }
                    // Error message
                    if (msg.SecsItem is { Format: not SecsFormat.List or SecsFormat.ASCII or SecsFormat.JIS8 } dataItem
                        && dataItem.GetReadOnlyMemory<byte>() is { Length: >= 10 } headerBytes)
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
                if (_replyExpectedMsgs.TryGetValue(id, out var token))
                {
                    token.completeSource.HandleReplyMessage(token.primary, msg);
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
                return;
            }

            _cancellationSourceForDataMessageProcessing.Cancel();
            _cancellationSourceForDataMessageProcessing.Dispose();
            _replyExpectedMsgs.Clear();
        }
    }
}
