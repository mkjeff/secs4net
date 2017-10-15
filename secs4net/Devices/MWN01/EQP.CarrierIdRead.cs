using Cim.Eap.Tx;
using Secs4Net;
namespace Eap.Driver.MWN {
    partial class Driver {
        void EQP_CarrierIDRead(SecsMessage msg) {
            var portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            var carrierId = msg.SecsItem.Items[2].Items[0].Items[1].Items[1].GetString().Trim();
            EAP.Report(new CarrierIDReport {
                LoadPortId = GetPortID(portNo),
                CarrierId = carrierId
            });
        }
    }
}