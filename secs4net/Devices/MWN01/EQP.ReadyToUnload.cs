using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_ReadyToUnload(SecsMessage msg) {
            EAP.Report(new ReadyToUnloadReport {
                PortID = GetPortID(msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>())
            });
        }
    }
}