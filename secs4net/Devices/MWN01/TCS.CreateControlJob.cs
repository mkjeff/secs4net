using System.Collections.Generic;
using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using Cim.Eap;

namespace Eap.Driver.MWN {
    partial class Driver {
        async Task TCS_CreateControlJob(CreateControlJobRequest tx) {
            var s14f10 = await EAP.SendAsync(new SecsMessage(14, 9, "CreateControlJob",
                Item.L(
                    Item.A("Equipment"),
                    Item.A("ControlJob"),
                    Item.L(
                        Item.L(
                            Item.A("ObjID"),
                            Item.A(tx.ControlJobID)),
                        Item.L(
                            Item.A("ProcessingCtrlSpec"),
                            Item.L(from pjid in tx.ProcessJobIDs select
                                Item.A(pjid))),
                        Item.L(
                            Item.A("CarrierInputSpec"),
                            Item.L(from carrier in tx.CarrierIDs select
                                Item.A(carrier))),
                        Item.L(
                            Item.A("MtrlOutSpec"),
                            Item.A()),
                        Item.L(
                            Item.A("ProcessOrderMgmt"),
                            Item.A("LIST")),
                        Item.L(
                            Item.A("StartMethod"),
                            Item.Boolean(true))))));

            byte returnCode = (byte)s14f10.SecsItem.Items[2].Items[0];
            if (returnCode != 0 && returnCode != 4) {
                await DeleteProcessJob(tx.ProcessJobIDs);
                throw new ScenarioException("S14F10_CreateControlJob_Handler Return Code:" + returnCode);
            }
        }

        async Task DeleteProcessJob(IEnumerable<string> processJobIds) {
            foreach (string pjID in processJobIds) {
                _ProcessingJobs.Remove(pjID);
                try {
                    await EAP.SendAsync(new SecsMessage(16, 5, "PJCancel",
                        Item.L(
                            Item.U4(0),
                            Item.A(pjID),
                            Item.A("Cancel"),
                            Item.L())));
                } catch { }
            }
        }
    }
}