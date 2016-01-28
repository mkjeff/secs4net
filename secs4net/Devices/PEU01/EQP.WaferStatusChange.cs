using System;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_WaferStatusChange(SecsMessage msg) {
            Item dataList = msg.SecsItem.Items[2].Items[0].Items[1];
            //string pjId = dataList.Items[0].ToString();//機台會報錯

            string carrier_slot_port = (string)dataList.Items[1];
            int i = carrier_slot_port.IndexOf('.');
            string carrierId = carrier_slot_port.Substring(0, i); 
            byte slotNo = Convert.ToByte(carrier_slot_port.Substring(i + 1, 2));

            string state = dataList.Items[2].ToString();
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