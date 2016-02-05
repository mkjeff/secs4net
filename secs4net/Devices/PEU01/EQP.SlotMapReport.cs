using Secs4Net;
using Cim.Eap.Tx;
using System.Linq;
namespace Cim.Eap {
    partial class Driver {
        void EQP_SlotMapReport(SecsMessage msg) {
            byte portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string carrierId = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[1];
            string map = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[1 + portNo];
        }
    }
}