using System.Diagnostics;
namespace Cim.Eap.Data {
    [DebuggerDisplay("{SlotNo} {Measure? \"[m]\" : \"\" ,nq}", Name = "{WaferID,nq}")]
    public struct SlotInfo {
        public byte SlotNo;
        public string WaferID;
        public bool Measure;
    }
}
