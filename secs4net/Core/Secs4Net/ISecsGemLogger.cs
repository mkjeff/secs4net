using System;
using System.Diagnostics;

namespace Secs4Net
{
    public interface ISecsGemLogger
    {
        void MessageIn(SecsMessage msg, int id) => Trace.WriteLine($"<-- [0x{id:X8}] {msg}");
        void MessageOut(SecsMessage msg, int id) => Trace.WriteLine($"--> [0x{id:X8}] {msg}");
        void Debug(string msg) => Trace.WriteLine(msg);
        void Info(string msg) => Trace.TraceInformation(msg);
        void Warning(string msg) => Trace.TraceWarning(msg);
        void Error(string msg) => Error(msg, message: null, ex: null);
        void Error(string msg, Exception ex) => Error(msg, message:null, ex);
        void Error(string msg, SecsMessage? message, Exception? ex) => Trace.TraceError($"{msg} {message}\n {ex}");
    }
}
