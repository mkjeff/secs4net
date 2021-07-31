using System.Linq;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(CreateControlJobRequest tx) {
            var s14f10 = await EAP.SendAsync(new SecsMessage(14, 9, "CreaeteControlJob",
                Item.L(
                    Item.A("Equipment"),
                    Item.A("ControlJob"),
                    Item.L(
                        Item.L(
                            Item.A("ObjID"),
                            Item.A(tx.ControlJobID)),
                        Item.L(
                            Item.A("CarrierInputSpec"),
                            Item.L(from carrier in tx.CarrierIDs select 
                                Item.A(carrier))),
                        Item.L(
                            Item.A("ProcessingCtrlSpec"),
                            Item.L(from pj in tx.ProcessJobIDs select
                                Item.L(
                                    Item.A(pj),
                                    Item.L(),
                                    Item.A()))),
                        Item.L(
                            Item.A("ProcessOrderMgmt"),
                            Item.U1(0)),
                        Item.L(
                            Item.A("StartMethod"),
                            Item.Boolean(true))))));
            var returnCode = (byte)s14f10.SecsItem.Items[2].Items[0];
            if (returnCode != 0 && returnCode != 4) {
                throw new ScenarioException("CreateControlJob fail. ");
            }
        }
    }
}