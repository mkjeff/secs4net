using System.Collections.Generic;
using System.Diagnostics;
namespace Cim.Eap.Data {
    [DebuggerDisplay("{Id,nq}")]
    public struct Carrier {
        public string Id;
        public string LoadPurposeType;
        public string LoadPortId;
        public IEnumerable<SlotInfo> SlotMap;
        internal IDictionary<byte,byte> _MeasurementSlots;// <實際量測slot,帳上mslot>
    }
}
