using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
namespace Eap.Driver.MWN {
    partial class Driver {
        void EQP_SlotMapReport(SecsMessage msg) {
            byte portNo = (byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string carrierID = msg.SecsItem.Items[2].Items[0].Items[1].Items[1].GetString().Trim();
            var mapItem = msg.SecsItem.Items[2].Items[0].Items[1].Items[2].Items;
            EAP.Report(new SlotMapReport {
                LoadPortID = GetPortID(portNo),
                CarrierID = carrierID,
                Slots = from i in Enumerable.Range(0, mapItem.Count)
                        where mapItem[i].GetValue<byte>() == 4
                        select (byte)(i + 1)
            });
        }
    }
}