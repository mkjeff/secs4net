
using System.Diagnostics;
namespace Cim.Eap.Data {
    [DebuggerDisplay("{LotID,nq}")]
    public struct EDALotInfo {
        public string LotID;
        public string ProductID;
        public string OperationNo;
        public string PassCount;
        public string FlowBatchID;
        public string Fab;
        public string CarrierID;
        public string RouteID;
    }
}
