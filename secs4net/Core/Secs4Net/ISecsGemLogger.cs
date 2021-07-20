using System;
using System.Diagnostics;

namespace Secs4Net
{
    public interface ISecsGemLogger
    {
        void MessageIn(SecsMessage msg, int systembyte) => Trace.WriteLine($"<-- [0x{systembyte:X8}] {msg}");
        void MessageOut(SecsMessage msg, int systembyte) => Trace.WriteLine($"--> [0x{systembyte:X8}] {msg}");
        void Debug(string msg) => Trace.WriteLine(msg);
        void Info(string msg) => Trace.TraceInformation(msg);
        void Warning(string msg) => Trace.TraceWarning(msg);
        void Error(string msg, Exception? ex = null) => Trace.TraceError($"{msg}\n {ex}");
    }
}
