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
        private readonly MessageHeader _header;
        public SecsMessage PrimaryMessage { get; }
        public SecsMessage? SecondaryMessage { get; private set; }
        public int MessageId => _header.SystemBytes;

        internal PrimaryMessageWrapper(SecsGem secsGem, MessageHeader header, SecsMessage primaryMessage)
        {
            _secsGem = new WeakReference<SecsGem>(secsGem);
            _header = header;
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

            if (!_secsGem.TryGetTarget(out var secsGem))
            {
                throw new SecsException("Hsms connector loss, the message has no chance to reply via the ReplyAsync method");
            }

            if (replyMessage is null)
            {
                var headerBytes = new byte[10];
                _header.EncodeTo(new MemoryBufferWriter<byte>(headerBytes));
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
                if(SecondaryMessage is not null)
                {
                    return false;
                }

                await secsGem.SendDataMessageAsync(replyMessage, replyMessage.S == 9 ? secsGem.NewSystemId : _header.SystemBytes);
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
