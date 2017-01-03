using System;
using System.Diagnostics;

namespace Secs4Net
{
    public interface ISecsGemLogger
    {
        void MessageIn(SecsMessage msg, int systembyte);
        void MessageOut(SecsMessage msg, int systembyte);

        void Debug(string msg);
        void Info(string msg);
        void Warning(string msg);
        void Error(string msg, Exception ex = null);
    }

    /// <summary>
    /// SECS Connector Logger
    /// </summary>
    sealed class DefaultSecsGemLogger : ISecsGemLogger
    {
        public void MessageIn(SecsMessage msg, int systembyte) {
            Trace.WriteLine($"<-- [0x{systembyte:X8}] {msg}");
        }

        public void MessageOut(SecsMessage msg, int systembyte) {
            Trace.WriteLine($"--> [0x{systembyte:X8}] {msg}");
        }

        public void Debug(string msg) {
            Trace.WriteLine(msg);
        }

        public void Info(string msg) {
            Trace.TraceInformation(msg);
        }

        public void Warning(string msg) {
            Trace.TraceWarning(msg);
        }

        public void Error(string msg, Exception ex = null) {
            Trace.TraceError($"{msg}\n {ex}");
        }
    }
}
