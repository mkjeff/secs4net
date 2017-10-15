using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
namespace Eap.Driver.MWN {
    partial class Driver {
        void EQP_WaferProcessData_LLH_LHC(SecsMessage msg) {
            Item tempList = msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string pjID = tempList.Items[1].GetString();
            byte slotNo = (byte)tempList.Items[4].Items[0].Items[1];

            tempList = tempList.Items[4].Items[0].Items[2].Items[0];

            var dc = new DataCollectionReport(GetProcessJob(pjID));
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
            Item tempList = msg.SecsItem.Items[2].Items[0].Items[1].Items[0];
            string pjID = tempList.Items[1].GetString();
            byte slotNo = (byte)tempList.Items[4].Items[0].Items[1];

            tempList = tempList.Items[4].Items[0].Items[2].Items[0];
            string chamber = tempList.Items[0].Items[1].GetString();

            var dc = new DataCollectionReport(GetProcessJob(pjID));
            try {
                dc.AddLotData("CIWW", chamber);

                
            } finally {
                EAP.Report(dc);
            }
        }
    }
}