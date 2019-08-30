namespace Secs4Net.Exceptions
{
	public class SecsSentMessageException :
		SecsException
	{
		public SecsSentMessageException(SecsMessage sentSecsMessage, string message)
			: base(message)
		{
			this.SentSecsMessage = sentSecsMessage;
		}

		public SecsMessage SentSecsMessage { get; }
	}
}