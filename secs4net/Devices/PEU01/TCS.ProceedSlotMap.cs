using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(ProceedSlotMapRequest tx) {
            var s3f18 = await EAP.SendAsync(new SecsMessage(3, 17, "ProceedWithCarrier",
                L(
                    U4(2),
                    A("ProceedWithCarrier"),
                    A(tx.Carrier.Id),
                    U1(GetPortNo(tx.Carrier.LoadPortId)),
                    L())));

            byte returnCode = (byte)s3f18.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("ProceedWithCarrier failed. ");
        }
    }
}