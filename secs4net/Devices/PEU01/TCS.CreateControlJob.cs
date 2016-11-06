using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(CreateControlJobRequest tx) {
            var s14f10 = await EAP.SendAsync(new SecsMessage(14, 9, "CreaeteControlJob",
                L(
                    A("Equipment"),
                    A("ControlJob"),
                    L(
                        L(
                            A("ObjID"),
                            A(tx.ControlJobID)),
                        L(
                            A("CarrierInputSpec"),
                            L(from carrier in tx.CarrierIDs select 
                                A(carrier))),
                        L(
                            A("ProcessingCtrlSpec"),
                            L(from pj in tx.ProcessJobIDs select
                                L(
                                    A(pj),
                                    L(),
                                    A()))),
                        L(
                            A("ProcessOrderMgmt"),
                            U1(0)),
                        L(
                            A("StartMethod"),
                            Boolean(true))))));
            byte returnCode = (byte)s14f10.SecsItem.Items[2].Items[0];
            if (returnCode != 0 && returnCode != 4) {
                throw new ScenarioException("CreateControlJob fail. ");
            }
        }
    }
}