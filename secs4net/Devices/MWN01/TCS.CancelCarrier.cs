using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task TCS_CancelCarrier(CancelCarrierRequest tx)
        {
            using (var s3f17 = CreateS3F17(tx))
            using (var s3f18 = await EAP.SendAsync(s3f17))
            {
                var returnCode = (byte) s3f18.SecsItem.Items[0];
                if (returnCode != 0 && returnCode != 4)
                    throw new ScenarioException($"CancelCarrier failed : {s3f18.SecsItem.Items[1].Items[0].Items[1].GetString()}");
            }
        }

        private static SecsMessage CreateS3F17(CancelCarrierRequest tx) =>
            new SecsMessage(3,
                            17,
                            "CancelCarrier",
                            L(
                              U4(0),
                              A(string.IsNullOrEmpty(tx.Carrier.Id) ? "CancelCarrierAtPort" : "CancelCarrier"),
                              A(string.IsNullOrEmpty(tx.Carrier.Id) ? string.Empty : tx.Carrier.Id),
                              B(GetPortNo(tx.Carrier.LoadPortId)),
                              L()));
    }
}