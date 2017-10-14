
using System;
using static System.Diagnostics.Trace;

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
    public sealed class DefaultSecsGemLogger : ISecsGemLogger
    {
        public void MessageIn(SecsMessage msg, int systembyte) => WriteLine($"<-- [0x{systembyte:X8}] {msg}");

        public void MessageOut(SecsMessage msg, int systembyte) => WriteLine($"--> [0x{systembyte:X8}] {msg}");

        public void Debug(string msg) => WriteLine(msg);

        public void Info(string msg) => TraceInformation(msg);

        public void Warning(string msg) => TraceWarning(msg);

        public void Error(string msg, Exception ex = null) => TraceError($"{msg}\n {ex}");
    }
}
