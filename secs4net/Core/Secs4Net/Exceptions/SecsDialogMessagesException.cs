namespace Secs4Net.Exceptions
{
	public class SecsDialogMessagesException :
		SecsSentMessageException
	{
		public SecsDialogMessagesException(SecsMessage sentSecsMessage, SecsMessage replySecsMessage, string message)
			: base(sentSecsMessage, message)
		{
			this.ReplySecsMessage = replySecsMessage;
		}

		public SecsMessage ReplySecsMessage { get; }
	}
}