using System.Linq;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(CreateProcessJobRequest tx) {
            var s16f16 = await EAP.SendAsync(new SecsMessage(16, 15, "CreateProcessJob",
                L(
                    U4(0),
                    L(from pj in tx.ProcessJobs select
                        L(
                            A(pj.Id),
                            B(2),
                            L(from carrier in pj.Carriers select
                                L(
                                    A(carrier.Id),
                                    L(from slot in carrier.SlotMap select
                                        U1(slot.SlotNo)))),
                            L(
                                A("STANDARD"),
                                A(pj.RecipeId),
                                L()),
                            Boolean(true),
                            L())))));

            if (!(bool)s16f16.SecsItem.Items[1].Items[0])
                throw new ScenarioException("CreateProcessJob fail. ");

            foreach (var processJob in tx.ProcessJobs)
                this._ProcessingJobs.Add(processJob);

        }
    }
}