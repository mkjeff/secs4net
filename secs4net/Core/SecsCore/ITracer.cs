
using System;

namespace Secs4Net
{
    /// <summary>
    /// SECS Connector Logger
    /// </summary>
    public class SecsGemLogger
    {
        public virtual void TraceMessageIn(SecsMessage msg, int systembyte) { }
        public virtual void TraceMessageOut(SecsMessage msg, int systembyte) { }
        public virtual void TraceDebug(string msg) { }
        public virtual void TraceInfo(string msg) { }
        public virtual void TraceWarning(string msg) { }
        public virtual void TraceError(string msg, Exception ex = null) { }
    }
}
