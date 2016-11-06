using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_WaferProcessData_LLH_LHC(SecsMessage msg) {
            var tempList = msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            var jobId = tempList.Items[1].GetString();
            var slotNo = tempList.Items[4].Items[0].Items[1].GetValue<byte>();

            tempList = tempList.Items[4].Items[0].Items[2].Items[0];

            var dc = new DataCollectionReport(GetProcessJob(jobId));
            try {
                dc.AddWaferData("CIWD", slotNo, tempList.Items[0].Items[1]);

                tempList = tempList.Items[2].Items[0].Items[2];

                dc.AddWaferData("C1W1", slotNo, tempList.Items[3]);
                dc.AddWaferData("C1W2", slotNo, tempList.Items[6]);
                dc.AddWaferData("C2W1", slotNo, tempList.Items[9]);
                dc.AddWaferData("C2W2", slotNo, tempList.Items[12]);
            } finally {
                EAP.Report(dc);
            }
        }

        void EQP_WaferProcessData_PVD(SecsMessage msg) {
            var tempList = msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            var pjID = tempList.Items[1].GetString();
            var slotNo = tempList.Items[4].Items[0].Items[1].GetValue<byte>();
            var chamber = tempList.Items[4].Items[0].Items[2].Items[0].Items[0].Items[1].GetString();

            var dc = new DataCollectionReport(GetProcessJob(pjID));
            try {
                dc.AddLotData("CIWW", chamber);

                
            } finally {
                EAP.Report(dc);
            }
        }
    }
}