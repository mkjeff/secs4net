
namespace Secs4Net {
    public class SecsTracer {
        public virtual void TraceMessageIn(SecsMessage msg, int systembyte) { }
        public virtual void TraceMessageOut(SecsMessage msg, int systembyte) { }
        public virtual void TraceInfo(string msg) { }
        public virtual void TraceWarning(string msg) { }
        public virtual void TraceError(string msg) { }
    }
}
