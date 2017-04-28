using System;
using System.Threading;
using System.Threading.Tasks;

namespace Secs4Net
{
    public sealed class PrimaryMessageWrapper
    {
        private int _isReplied = 0;
        private readonly WeakReference<SecsGem> _secsGem;
        private readonly MessageHeader _header;
        public SecsMessage Message { get; }
        public int MessageId => _header.SystemBytes;

        internal PrimaryMessageWrapper(SecsGem secsGem, MessageHeader header, SecsMessage msg)
        {
            _secsGem = new WeakReference<SecsGem>(secsGem);
            _header = header;
            Message = msg;
        }

        /// <summary>
        /// Each PrimaryMessageWrapper can invoke Reply method once.
        /// Since message replied, method return false.
        /// </summary>
        /// <param name="replyMessage"></param>
        /// <returns>true, if reply message sent.</returns>
        public async Task<bool> ReplyAsync(SecsMessage replyMessage)
        {
            if (Interlocked.Exchange(ref _isReplied, 1) == 1)
                return false;

            if (!Message.ReplyExpected || !_secsGem.TryGetTarget(out var secsGem))
                return true;

            replyMessage = replyMessage ?? new SecsMessage(9, 7, false, "Unknown Message", Item.B(_header.Bytes));
            replyMessage.ReplyExpected = false;

            await secsGem.SendDataMessageAsync(replyMessage, replyMessage.S == 9 ? secsGem.NewSystemId : _header.SystemBytes);

            return true;
        }

        public override string ToString() => Message.ToString();
    }
}
