using System;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_WaferStatusChange(SecsMessage msg) {
            var dataList = msg.SecsItem.Items[2].Items[0].Items[1];
            var pjId = dataList.Items[0].GetString();

            var carrier_slot_port = (string)dataList.Items[1];
            var i = carrier_slot_port.IndexOf('.');
            var carrierId = carrier_slot_port.Substring(0, i); 
            var slotNo = Convert.ToByte(carrier_slot_port.Substring(i + 1, 2));

            var state = dataList.Items[2].GetString();
            switch (state) {
                case "SubstLocChamber":
                    EAP.Report(new WaferStartReport {
                        ProcessJobId = GetProcessJob(carrierId,slotNo).Id,
                        CarrierId = carrierId,
                        SlotNo = slotNo
                    });
                    break;
                case "SubstLocLowerArm":
                    EAP.Report(new WaferEndReport {
                        ProcessJobId = GetProcessJob(carrierId,slotNo).Id,
                        CarrierId = carrierId,
                        SlotNo = slotNo
                    });
                    break;
            }
        }
    }
}