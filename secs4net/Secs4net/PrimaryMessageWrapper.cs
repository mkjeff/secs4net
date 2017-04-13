using System;
using System.Threading;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Secs4Net
{
    public sealed class PrimaryMessageWrapper
    {
        private readonly WeakReference<SecsGem> _secsGem;
        private readonly MessageHeader _header;

        /// <summary>
        /// Received message, noted: the message will be recycled when wrapper has been finalized. 
        /// Don't keep the reference of the property, just handle the message's content.
        /// </summary>
        public SecsMessage Message { get; }
        public int MessageId => _header.SystemBytes;

        internal PrimaryMessageWrapper(SecsGem secsGem, MessageHeader header, SecsMessage msg)
        {
            _secsGem = new WeakReference<SecsGem>(secsGem);
            _header = header;
            Message = msg;
        }

        /// <summary>
        /// Reply message to device
        /// </summary>
        /// <param name="replyMessage"></param>
        /// <param name="autoDispose">auto disposes message when sent<paramref name="replyMessage"/></param>
        /// <returns>true, if reply message sent.</returns>
        public async ValueTask<bool> ReplyAsync(SecsMessage replyMessage, bool autoDispose = true)
        {
            if (!Message.ReplyExpected || !_secsGem.TryGetTarget(out var secsGem))
                return false;

            if (replyMessage is null)
                replyMessage = new SecsMessage(9, 7, false, "Unknown Message", B(_header.EncodeTo(new byte[10])));
            else
                replyMessage.ReplyExpected = false;

            await secsGem.SendDataMessageAsync(replyMessage,
                replyMessage.S == 9 ? secsGem.NewSystemId : _header.SystemBytes, autoDispose).ConfigureAwait(false);

            return true;
        }

        ~PrimaryMessageWrapper() => Message.Dispose();

        public override string ToString() => Message.ToString();
    }
}
