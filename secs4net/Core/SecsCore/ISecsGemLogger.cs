
using System;
using System.Diagnostics;

namespace Secs4Net
{
    public interface ISecsGemLogger
    {
        void TraceMessageIn(SecsMessage msg, int systembyte);
        void TraceMessageOut(SecsMessage msg, int systembyte);

        void TraceDebug(string msg);
        void TraceInfo(string msg);

        void TraceWarning(string msg);

        void TraceError(string msg, Exception ex = null);
    }

    /// <summary>
    /// SECS Connector Logger
    /// </summary>
    sealed class SecsGemLogger : ISecsGemLogger
    {
        public void TraceMessageIn(SecsMessage msg, int systembyte) {
            Trace.WriteLine($"<-- [0x{systembyte:X8}] {msg}");
        }

        public void TraceMessageOut(SecsMessage msg, int systembyte) {
            Trace.WriteLine($"--> [0x{systembyte:X8}] {msg}");
        }

        public void TraceDebug(string msg) {
            Trace.WriteLine(msg);
        }

        public void TraceInfo(string msg) {
            Trace.TraceInformation(msg);
        }

        public void TraceWarning(string msg) {
            Trace.TraceWarning(msg);
        }

        public void TraceError(string msg, Exception ex = null) {
            Trace.TraceError($"{msg}\n {ex}");
        }
    }
}
