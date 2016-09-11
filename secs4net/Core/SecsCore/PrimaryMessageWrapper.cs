using System;

namespace Secs4Net
{
    public class PrimaryMessageWrapper
    {
        public int MessageId { get; }
        public SecsMessage Message { get; }

        bool _isReplied;

        readonly Action<SecsMessage> _replyCallback;

        internal PrimaryMessageWrapper(int systemId, SecsMessage msg,Action<SecsMessage> replyCallback)
        {
            MessageId = systemId;
            Message = msg;
            _replyCallback = replyCallback;
        }

        /// <summary>
        /// Each PrimaryMessageWrapper can invoke Reply method once.
        /// Since message replied, method return false.
        /// </summary>
        /// <param name="replyMessage"></param>
        /// <returns>ture, if reply message sent.</returns>
        public bool Reply(SecsMessage replyMessage)
        {
            if (_isReplied)
                return false;
            _replyCallback(replyMessage);
            _isReplied = true;
            return true;
        }

    }
}
