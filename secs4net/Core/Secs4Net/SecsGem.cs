using Microsoft.Extensions.Options;
using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Secs4Net
{
    public interface ISecsGem
    {
        /// <summary>
        /// Get async-stream of primary messages
        /// </summary>
        /// <param name="cancellation"></param>
        IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Asynchronously send message to device.
        /// </summary>
        /// <param name="message">primary message</param>
        /// <returns>Secondary message, or null if <paramref name="message"/>'s ReplyExpected is false</returns>
        Task<SecsMessage?> SendAsync(SecsMessage message, CancellationToken cancellation = default);
    }

    public sealed class SecsGem : ISecsGem, IDisposable
    {
        private const int DisposalNotStarted = 0;
        private const int DisposalComplete = 1;
        private int _disposeStage;
        private readonly ISecsGemLogger _logger;
        private readonly IHsmsConnection _hsmsConnector;

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

        private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new();
        private readonly CancellationTokenSource _cancellationTokenSourceForDecoderLoop = new();

        public SecsGem(IOptions<SecsGemOptions> secsGemOptions, IHsmsConnection hsmsConnector, ISecsGemLogger logger)
        {
            var options = secsGemOptions.Value;
            DeviceId = options.DeviceId;
            T3 = options.T3;

            _hsmsConnector = hsmsConnector;
            _logger = logger;

            var cancellation = _cancellationTokenSourceForDecoderLoop.Token;
            _ = AsyncHelper.LongRunningAsync(() =>
                StartDataMessagesConsumer(_hsmsConnector.PipeDecoder.GetDataMessages(cancellation), cancellation), cancellation);
        }

        internal async Task<SecsMessage?> SendDataMessageAsync(SecsMessage msg, int systembyte, CancellationToken cancellation)
        {
            if (_hsmsConnector.State != ConnectionState.Selected)
            {
                throw new SecsException("Device is not selected");
            }

            msg.DeviceId = DeviceId;
            msg.Id = systembyte;
            var token = new TaskCompletionSourceToken(msg);
            if (msg.ReplyExpected)
            {
                _replyExpectedMsgs[systembyte] = token;
            }

            try
            {
                await EncodeAndSendMessageAsync(msg, cancellation).ConfigureAwait(false);

                _logger.MessageOut(msg, msg.Id);

                if (!msg.ReplyExpected)
                {
                    return null;
                }

                return await token.Task.WaitAsync(TimeSpan.FromMilliseconds(T3), cancellation).ConfigureAwait(false);
            }
            catch (SocketException)
            {
                _hsmsConnector.Reconnect();
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

            async Task EncodeAndSendMessageAsync(SecsMessage msg, CancellationToken cancellation)
            {
                using var buffer = new ArrayPoolBufferWriter<byte>(initialCapacity: 256);
                EncodeMessage(msg, buffer);
                await HsmsConnection.SendAsync(_hsmsConnector, buffer.WrittenMemory, cancellation).ConfigureAwait(false);
            }
        }

        internal static void EncodeMessage(SecsMessage msg, ArrayPoolBufferWriter<byte> buffer)
        {
            // reserve 4 byte for total length
            var lengthBytes = buffer.GetSpan(sizeof(int)).Slice(0, sizeof(int));
            buffer.Advance(sizeof(int));

            msg.EncodeHeaderTo(buffer);
            msg.SecsItem?.EncodeTo(buffer);

            BinaryPrimitives.WriteInt32BigEndian(lengthBytes, buffer.WrittenCount - sizeof(int));
        }

        public Task<SecsMessage?> SendAsync(SecsMessage msg, CancellationToken cancellation = default)
            => SendDataMessageAsync(msg, SystemByteGenerator.New(), cancellation);

        public IAsyncEnumerable<PrimaryMessageWrapper> GetPrimaryMessageAsync(CancellationToken cancellation = default)
            => _primaryMessageChannel.Reader.ReadAllAsync(cancellation);

        private async Task StartDataMessagesConsumer(IAsyncEnumerable<SecsMessage> messages, CancellationToken cancellation)
        {
            await foreach (var msg in messages)
            {
                var systembyte = msg.Id;
                if (msg.DeviceId != DeviceId && msg.S != 9 && msg.F != 1)
                {
                    _logger.MessageIn(msg, systembyte);
                    _logger.Warning("Received Unrecognized Device Id Message");
                    var headerBytes = new byte[10];
                    msg.EncodeHeaderTo(new MemoryBufferWriter<byte>(headerBytes));
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
                        await _primaryMessageChannel.Writer.WriteAsync(new PrimaryMessageWrapper(this, msg), cancellation).ConfigureAwait(false);
                        continue;
                    }
                    // Error message
                    if (msg.SecsItem?.GetValues<byte>() is not { Length: 10 } headerBytes)
                    {
                        _logger.Warning("Can't get expected header bytes");
                    }
                    else
                    {
                        systembyte = BinaryPrimitives.ReadInt32BigEndian(headerBytes.AsSpan().Slice(6, 4));
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

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposeStage, DisposalComplete) != DisposalNotStarted)
            {
                return;
            }

            _cancellationTokenSourceForDecoderLoop.Cancel();
            _cancellationTokenSourceForDecoderLoop.Dispose();
            _replyExpectedMsgs.Clear();
        }

        private sealed class TaskCompletionSourceToken : TaskCompletionSource<SecsMessage>
        {
            public SecsMessage MessageSent { get; }

            internal TaskCompletionSourceToken(SecsMessage primaryMessageMsg)
                : base(TaskCreationOptions.RunContinuationsAsynchronously)
            {
                MessageSent = primaryMessageMsg;
            }

            public void HandleReplyMessage(SecsMessage replyMsg)
            {
                replyMsg.Name = MessageSent.Name;
                if (replyMsg.F == 0)
                {
                    TrySetException(new SecsException(MessageSent, Resources.SxF0));
                    return;
                }

                if (replyMsg.S == 9)
                {
                    switch (replyMsg.F)
                    {
                        case 1:
                            TrySetException(new SecsException(MessageSent, Resources.S9F1));
                            break;
                        case 3:
                            TrySetException(new SecsException(MessageSent, Resources.S9F3));
                            break;
                        case 5:
                            TrySetException(new SecsException(MessageSent, Resources.S9F5));
                            break;
                        case 7:
                            TrySetException(new SecsException(MessageSent, Resources.S9F7));
                            break;
                        case 9:
                            TrySetException(new SecsException(MessageSent, Resources.S9F9));
                            break;
                        case 11:
                            TrySetException(new SecsException(MessageSent, Resources.S9F11));
                            break;
                        case 13:
                            TrySetException(new SecsException(MessageSent, Resources.S9F13));
                            break;
                        default:
                            TrySetException(new SecsException(MessageSent, Resources.S9Fy));
                            break;
                    }
                    return;
                }

                TrySetResult(replyMsg);
            }
        }
    }
}
