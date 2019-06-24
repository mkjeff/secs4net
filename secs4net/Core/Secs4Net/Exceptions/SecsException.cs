using System;

namespace Secs4Net
{
	public sealed class SecsException : Exception
	{
		public SecsMessage SecsMsg { get; }

		public SecsException(SecsMessage msg, string description)
			: base(description)
		{
			this.SecsMsg = msg;
		}

		public SecsException(string msg)
			: this(null, msg)
		{
		}
	}
}