using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        void EQP_ProcessJobEnd(SecsMessage msg) {
            var id = msg.SecsItem.Items[2].Items[0].Items[1].Items[0].GetString().Trim();
            EAP.Report(new ProcessJobEndReport { ProcessJobID = id });
            EAP.Report(new DataCollectionCompleteReport(GetProcessJob(id)));
        }  
    }
}