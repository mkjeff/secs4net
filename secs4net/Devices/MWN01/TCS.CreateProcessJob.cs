using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;
namespace Cim.Eap
{
    partial class Driver
    {
        async Task TCS_CreateProcessJob(CreateProcessJobRequest tx)
        {
            using (var s16f16 = await EAP.SendAsync(GetS16F15(ref tx)))
            {
                var returnCode = (bool)s16f16.SecsItem.Items[1].Items[0];
                if (!returnCode)
                    throw new ScenarioException($"CreateProcessJob fail");

                foreach (var processJob in tx.ProcessJobs)
                    _ProcessingJobs[processJob.Id] = processJob;
            }
        }

        private static SecsMessage GetS16F15(ref CreateProcessJobRequest tx)
            => new SecsMessage(16, 15, "CreateProcessJob",
                   L(
                       U4(0),
                       L(from pj in tx.ProcessJobs
                         select
                         L(
                             A(pj.Id),
                             B(0x0D),
                             L(from carrier in pj.Carriers
                               select
                               L(
                                   A(carrier.Id),
                                   L(from slotInfo in carrier.SlotMap
                                     select
                                     U1(slotInfo.SlotNo)))),
                             L(
                                 U1(1),
                                 A(pj.RecipeId),
                                 L()),
                             Boolean(true),
                             L()))));
    }
}