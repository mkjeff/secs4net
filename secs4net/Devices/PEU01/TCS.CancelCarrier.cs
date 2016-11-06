using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(CancelCarrierRequest tx) {
            bool isUnknown = tx.Carrier.Id == "(Unknown)";
            var s3f18 = await EAP.SendAsync(new SecsMessage(3, 17, "CancelCarrier",
                L(
                    U4(0),
                    A(isUnknown?"CancelCarrierAtPort":"CancelCarrier"),
                    A(isUnknown?string.Empty:tx.Carrier.Id),
                    U1(GetPortNo(tx.Carrier.LoadPortId)),
                    L())));

            byte returnCode = (byte)s3f18.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("CancelCarrier failed. ");
        }
    }
}