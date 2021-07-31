using System;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_DataCollectionComplete(SecsMessage msg) {
            var tmpList = msg.SecsItem.Items[2].Items[0].Items[1];

            var carrier_slot_port = (string)tmpList.Items[1];
            var i = carrier_slot_port.IndexOf('.');
            var carrierId = carrier_slot_port.Substring(0, i);
            var slotNo = Convert.ToByte(carrier_slot_port.Substring(i + 1, 2));
            var portNo = carrier_slot_port[carrier_slot_port.Length - 1];

            var processJob = GetProcessJob(carrierId, slotNo);
            var dc = new DataCollectionReport(processJob);
            var dcc = new DataCollectionCompleteReport(processJob);
            try {
                dc.AddLotData("PJL0", processJob.Id);
                dc.AddLotData("CJL0", carrierId);
                dc.AddLotData("SNL0", slotNo);
                dc.AddLotData("PIL0", Convert.ToByte(portNo));
                dc.AddLotData("RIL0", tmpList.Items[2]);
                dc.AddLotData("LTL0", (uint)tmpList.Items[3] / 100);
            } finally {
                EAP.Report(dc);
                EAP.Report(dcc);
            }
        }
    }
}