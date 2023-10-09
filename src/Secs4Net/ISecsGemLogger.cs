namespace Secs4Net;

public interface ISecsGemLogger
{
#if NET
    void MessageIn(SecsMessage msg, int id) => Trace.WriteLine($"<-- [0x{id:X8}] {msg}");
    void MessageOut(SecsMessage msg, int id) => Trace.WriteLine($"--> [0x{id:X8}] {msg}");
    void Debug(string msg) => Trace.WriteLine(msg);
    void Info(string msg) => Trace.TraceInformation(msg);
    void Warning(string msg) => Trace.TraceWarning(msg);
    void Error(string msg) => Error(msg, message: null, ex: null);
    void Error(string msg, Exception ex) => Error(msg, message: null, ex);
    void Error(string msg, SecsMessage? message, Exception? ex) => Trace.TraceError($"{msg} {message}\n {ex}");
#else
    void MessageIn(SecsMessage msg, int id);
    void MessageOut(SecsMessage msg, int id);
    void Debug(string msg);
    void Info(string msg);
    void Warning(string msg);
    void Error(string msg);
    void Error(string msg, Exception ex);
    void Error(string msg, SecsMessage? message, Exception? ex);
#endif
}
