using System;

namespace Secs4Net
{
    public class SecsException : Exception
    {
        public int? MessageId { get; }

        public SecsException(int messageId, string description) : base(description)
        {
            MessageId = messageId;
        }

        public SecsException(string msg) : base(msg) { }
    }
}