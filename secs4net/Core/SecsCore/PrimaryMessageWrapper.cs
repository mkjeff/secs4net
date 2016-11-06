using System;
using System.Buffers;
using System.Linq;
using System.Threading;
using static Secs4Net.Item;

namespace Secs4Net
{
    public sealed class PrimaryMessageWrapper
    {
        int _isReplied = 0;
        readonly SecsGem _secsGem;
        readonly MessageHeader _header;
        public SecsMessage Message { get; }
        public int MessageId => _header.SystemBytes;

        internal PrimaryMessageWrapper(SecsGem secsGem, MessageHeader header, SecsMessage msg)
        {
            _secsGem = secsGem;
            _header = header;
            Message = msg;
        }

        /// <summary>
        /// Each PrimaryMessageWrapper can invoke Reply method once.
        /// Since message replied, method return false.
        /// </summary>
        /// <param name="replyMessage"></param>
        /// <returns>ture, if reply message sent.</returns>
        public bool Reply(SecsMessage replyMessage)
        {
            if (Interlocked.Exchange(ref _isReplied, 1) == 1)
                return false;

            if (!Message.ReplyExpected)
                return true;

            var autoDispose = false;
            if (replyMessage == null)
            {
                autoDispose = true;
                var arr = ArrayPool<byte>.Shared.Rent(10);
                Buffer.BlockCopy(((ArraySegment<byte>) _header).Array, 0, arr, 0, 10);
                replyMessage = new SecsMessage(9,
                                               7,
                                               false,
                                               "Unknown Message",
                                               B(arr));
            }
            else
            {
                replyMessage.ReplyExpected = false;
            }

            _secsGem.SendDataMessageAsync(replyMessage,
                                          replyMessage.S == 9 ? _secsGem.NewSystemId : _header.SystemBytes,
                                          autoDispose);

            return true;
        }

        public override string ToString() => Message.ToString();
    }
}
