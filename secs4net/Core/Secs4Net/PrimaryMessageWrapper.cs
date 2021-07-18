using Microsoft.Toolkit.HighPerformance.Buffers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PrimaryMessageWrapper
    {
        private readonly SemaphoreSlim _semaphoreSlim = new(initialCount: 1);
        private readonly WeakReference<SecsGem> _secsGem;
        public SecsMessage PrimaryMessage { get; }
        public SecsMessage? SecondaryMessage { get; private set; }

        internal PrimaryMessageWrapper(SecsGem secsGem, SecsMessage primaryMessage)
        {
            _secsGem = new WeakReference<SecsGem>(secsGem);
            PrimaryMessage = primaryMessage;
        }

        /// <summary>
        /// If the message has already replied, method will return false.
        /// </summary>
        /// <param name="replyMessage">Reply S9F7 if parameter is null</param>
        /// <returns>true, if reply success.</returns>
        public async Task<bool> TryReplyAsync(SecsMessage? replyMessage, CancellationToken cancellation = default)
        {
            if (!PrimaryMessage.ReplyExpected)
            {
                throw new SecsException("The message does not need to reply");
            }

            if (!_secsGem.TryGetTarget(out var secsGem) || secsGem.IsDisposed)
            {
                throw new SecsException("Hsms connector loss, the message has no chance to reply via the ReplyAsync method");
            }

            if (replyMessage is null)
            {
                var headerBytes = new byte[10];
                var buffer = new MemoryBufferWriter<byte>(headerBytes);
                PrimaryMessage.EncodeTo(buffer);

                replyMessage = new SecsMessage(9, 7, replyExpected: false)
                {
                    Name = "Unknown Message",
                    SecsItem = Item.B(headerBytes),
                };
            }
            else
            {
                replyMessage.ReplyExpected = false;
            }

            await _semaphoreSlim.WaitAsync(cancellation).ConfigureAwait(false);
            try
            {
                if (SecondaryMessage is not null)
                {
                    return false;
                }

                int systembyte = replyMessage.S == 9 ? SystemByteGenerator.New() : PrimaryMessage.Id;
                await secsGem.SendDataMessageAsync(replyMessage, systembyte, cancellation).ConfigureAwait(false);
                SecondaryMessage = replyMessage;
                return true;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public override string ToString() => PrimaryMessage.ToString();
    }
}
