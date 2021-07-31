using System.Collections.Generic;
using System.Diagnostics;

namespace Cim.Eap.Data {
    [DebuggerDisplay("{Id}")]
    public struct ControlJob {
        public string Id;
        public bool HeadOfQueue;
        public bool StartedFlag;
        public bool EndFlag;
        public IEnumerable<ProcessJob> ProcessJobs;
    }
}
