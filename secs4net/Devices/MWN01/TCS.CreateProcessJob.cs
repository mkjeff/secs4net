using System.Linq;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using Cim.Eap;

namespace Eap.Driver.MWN {
    partial class Driver {
        async Task TCS_CreateProcessJob(CreateProcessJobRequest tx) {
            var s16f16 = await EAP.SendAsync(new SecsMessage(16, 15, "CreateProcessJob",
                Item.L(
                    Item.U4(0),
                    Item.L(from pj in tx.ProcessJobs select
                        Item.L(
                            Item.A(pj.Id),
                            Item.B(0x0D),
                            Item.L(from carrier in pj.Carriers select
                                Item.L(
                                    Item.A(carrier.Id),
                                    Item.L(from slotInfo in carrier.SlotMap select
                                        Item.U1(slotInfo.SlotNo)))),
                            Item.L(
                                Item.U1(1),
                                Item.A(pj.RecipeId),
                                Item.L()),
                            Item.Boolean(true),
                            Item.L())))));

            if (!s16f16.SecsItem.Items[1].Items[0])
                throw new ScenarioException("CreateProcessJob fail Return Code:" + s16f16.SecsItem.Items[1].Items[0]);

            foreach (var processJob in tx.ProcessJobs)
                this._ProcessingJobs[processJob.Id] = processJob;
        }
    }
}