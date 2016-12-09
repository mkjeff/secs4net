using Cim.Eap.Tx;
using Secs4Net;
namespace Eap.Driver.MWN {
    partial class Driver {
        void EQP_CarrierIDRead(SecsMessage msg) {
            byte portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string carrierID = msg.SecsItem.Items[2].Items[0].Items[1].Items[1].GetValue<string>().Trim();
            EAP.Report(new CarrierIDReport {
                LoadPortId = GetPortID(portNo),
                CarrierId = carrierID
            });
        }
    }
}