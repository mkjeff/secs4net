using System;
using System.Diagnostics;

namespace Secs4Net
{
	/// <summary>
	/// SECS Connector Logger
	/// </summary>
	public sealed class DefaultSecsGemLogger :
		ISecsGemLogger
	{
		public void MessageIn(SecsMessage secsMessage, int systembyte) => Trace.WriteLine($"<-- [0x{systembyte:X8}] {secsMessage}");

		public void MessageOut(SecsMessage secsMessage, int systembyte) => Trace.WriteLine($"--> [0x{systembyte:X8}] {secsMessage}");

		public void Debug(string message) => Trace.WriteLine(message);

		public void Info(string message) => Trace.TraceInformation(message);

		public void Warning(string message) => Trace.TraceWarning(message);

		public void Error(string message, Exception exception = null) => Trace.TraceError($"{message}{Environment.NewLine}{exception}");
	}
}