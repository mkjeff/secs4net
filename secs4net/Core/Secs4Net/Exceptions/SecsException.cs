using System;

namespace Secs4Net.Exceptions
{
	public class SecsException :
		Exception
	{
		public SecsException(string message)
			: this(null, message)
		{
		}

		public SecsException(SecsMessage sentSecsMessage, string message)
			: base(message)
		{
			this.SentSecsMessage = sentSecsMessage;
		}

		public SecsMessage SentSecsMessage { get; }
	}
}