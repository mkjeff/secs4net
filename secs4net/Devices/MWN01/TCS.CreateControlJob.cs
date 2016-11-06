using System.Collections.Generic;
using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap
{
    partial class Driver
    {
        async Task TCS_CreateControlJob(CreateControlJobRequest tx)
        {
            using (var s14f10 = await EAP.SendAsync(CreateS14F9(tx)))
            {

                var returnCode = (byte)s14f10.SecsItem.Items[2].Items[0];
                if (returnCode != 0 && returnCode != 4)
                {
                    await DeleteProcessJob(tx.ProcessJobIDs);
                    throw new ScenarioException($"S14F10_CreateControlJob_Handler Return Code:{returnCode}");
                }
            }
        }

        private static SecsMessage CreateS14F9(CreateControlJobRequest tx) =>
            new SecsMessage(14,
                            9,
                            "CreateControlJob",
                            L(
                              A("Equipment"),
                              A("ControlJob"),
                              L(
                                L(
                                  A("ObjID"),
                                  A(tx.ControlJobID)),
                                L(
                                  A(
                                    "ProcessingCtrlSpec"),
                                  L(
                                    from pjid in tx.ProcessJobIDs
                                    select
                                    A(pjid))),
                                L(
                                  A("CarrierInputSpec"),
                                  L(
                                    from carrier in tx.CarrierIDs
                                    select
                                    A(carrier))),
                                L(
                                  A("MtrlOutSpec"),
                                  A()),
                                L(
                                  A("ProcessOrderMgmt"),
                                  A("LIST")),
                                L(
                                  A("StartMethod"),
                                  Boolean(true)))));

        async Task DeleteProcessJob(IEnumerable<string> processJobIds)
        {
            foreach (var id in processJobIds)
            {
                _ProcessingJobs.Remove(id);
                try
                {
                    using (await EAP.SendAsync(CreateS16F5(id)))
                    {
                    }
                }
                catch
                {
                }
            }
        }

        private static SecsMessage CreateS16F5(string id) =>
            new SecsMessage(16,
                            5,
                            "PJCancel",
                            L(
                              U4(0),
                              A(id),
                              A("Cancel"),
                              L()));
    }
}