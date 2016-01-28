using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_CarrierIDReadFail(SecsMessage msg) {
            byte portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            EAP.Report(new CarrierIDReport { 
                LoadPortId = GetPortID(portNo)
            });
        }
    }
}