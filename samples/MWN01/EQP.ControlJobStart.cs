using Cim.Eap.Tx;
using Secs4Net;
namespace Eap.Driver.MWN {
    partial class Driver {
        void EQP_ControlJobStart(SecsMessage msg) {
            EAP.Report(new ControlJobStartReport {
                ControlJobID = msg.SecsItem.Items[2].Items[0].Items[1].Items[1].GetString().Trim()
            });
        }
    }
}