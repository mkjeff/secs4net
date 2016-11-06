using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_SlotMapReport(SecsMessage msg) {
            var portNo = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetValue<byte>();
            var carrierId = msg.SecsItem.Items[2].Items[0].Items[1].Items[1].GetString().Trim();
            var mapItem = msg.SecsItem.Items[2].Items[0].Items[1].Items[2].Items;
            EAP.Report(new SlotMapReport {
                LoadPortID = GetPortID(portNo),
                CarrierID = carrierId,
                Slots = from i in Enumerable.Range(0, mapItem.Count)
                        where mapItem[i].GetValue<byte>() == 4
                        select (byte)(i + 1)
            });
        }
    }
}