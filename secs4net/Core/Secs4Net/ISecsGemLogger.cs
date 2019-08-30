using System;

namespace Secs4Net
{
	public interface ISecsGemLogger
	{
		void MessageIn(SecsMessage secsMessage, int systembyte);

		void MessageOut(SecsMessage secsMessage, int systembyte);

		void Debug(string message);

		void Info(string message);

		void Warning(string message);

		void Error(string message, Exception exception = null);
	}
}