using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task TCS_ProceedSlotMap(ProceedSlotMapRequest tx)
        {
            using(var s3f17 = CreateS3F17(tx))
            using (var s3f18 = await EAP.SendAsync(s3f17))
            {
                byte returnCode = (byte) s3f18.SecsItem.Items[0];
                if (returnCode != 0 && returnCode != 4)
                    throw new ScenarioException(
                                                $"ProceedWithCarrier failed : {s3f18.SecsItem.Items[1].Items[0].Items[1].GetString()}");
            }
        }

        private static SecsMessage CreateS3F17(ProceedSlotMapRequest tx) =>
            new SecsMessage(3,
                            17,
                            "ProceedWithCarrier",
                            L(
                              U4(2),
                              A("ProceedWithCarrier"),
                              A(tx.Carrier.Id),
                              B(GetPortNo(tx.Carrier.LoadPortId)),
                              L()));
    }
}