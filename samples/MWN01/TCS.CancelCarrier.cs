using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using Cim.Eap;

namespace Eap.Driver.MWN {
    partial class Driver {
        async Task TCS_CancelCarrier(CancelCarrierRequest tx) {
            bool isUnknown = tx.Carrier.Id == "(Unknown)";
            var s3f18 = await EAP.SendAsync(new SecsMessage(3, 17, "CancelCarrier",
                Item.L(
                    Item.U4(0),
                    Item.A(isUnknown ? "CancelCarrierAtPort" : "CancelCarrier"),
                    Item.A(isUnknown ? string.Empty : tx.Carrier.Id),
                    Item.B(GetPortNo(tx.Carrier.LoadPortId)),
                    Item.L())));

            byte returnCode = (byte)s3f18.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("CancelCarrier failed : " + s3f18.SecsItem.Items[1].Items[0].Items[1].GetString());
        }
    }
}