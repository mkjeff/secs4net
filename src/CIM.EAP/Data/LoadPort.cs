
using System.Diagnostics;
namespace Cim.Eap.Data {
    [DebuggerDisplay("{AccessMode}",Name="{Id,nq}")]
    public struct LoadPort {
        public string Id;
        public AccessMode AccessMode;
    }
}
