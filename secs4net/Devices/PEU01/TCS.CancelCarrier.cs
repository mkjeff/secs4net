using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(CancelCarrierRequest tx) {
            var isUnknown = tx.Carrier.Id == "(Unknown)";
            var s3f18 = await EAP.SendAsync(new SecsMessage(3, 17, "CancelCarrier",
                Item.L(
                    Item.U4(0),
                    Item.A(isUnknown?"CancelCarrierAtPort":"CancelCarrier"),
                    Item.A(isUnknown?string.Empty:tx.Carrier.Id),
                    Item.U1(GetPortNo(tx.Carrier.LoadPortId)),
                    Item.L())));

            var returnCode = (byte)s3f18.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("CancelCarrier failed. ");
        }
    }
}