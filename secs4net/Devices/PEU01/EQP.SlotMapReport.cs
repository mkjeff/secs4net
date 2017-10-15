using Secs4Net;
using Cim.Eap.Tx;
using System.Linq;
namespace Cim.Eap {
    partial class Driver {
        void EQP_SlotMapReport(SecsMessage msg) {
            var portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            var carrierId = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[1];
            var map = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[1 + portNo];
            EAP.Report(new SlotMapReport {
                LoadPortID = GetPortID(portNo),
                CarrierID = carrierId,
                Slots = from i in Enumerable.Range(0, map.Length)
                        where map[i] == '1'
                        select (byte)(i + 1)
            });
        }
    }
}