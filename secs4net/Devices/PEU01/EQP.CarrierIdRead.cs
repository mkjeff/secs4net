using Secs4Net;
using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void EQP_CarrierIdRead(SecsMessage msg) {
            EAP.Report(new CarrierIDReport {
                LoadPortId = GetPortID((byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0]),
                CarrierId = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[1]
            });
        }
    }
}