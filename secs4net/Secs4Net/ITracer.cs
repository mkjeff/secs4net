
namespace Secs4Net {
    public interface ISecsTracer {
        void TraceMessageIn(SecsMessage msg, int systembyte);
        void TraceMessageOut(SecsMessage msg, int systembyte);
        void TraceInfo(string msg);
        void TraceWarning(string msg);
        void TraceError(string msg);
    }
}
