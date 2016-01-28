using Secs4Net;
using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void EQP_ProcessJobEnd(SecsMessage msg) {
            EAP.Report(new ProcessJobEndReport {
                ProcessJobID = (string)msg.SecsItem.Items[2].Items[0].Items[1].Items[0]
            });
        }
    }
}