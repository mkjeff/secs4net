﻿using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        async void TCS_ProceedSlotMap(ProceedSlotMapRequest tx) {
            var s3f18 = await EAP.SendAsync(new SecsMessage(3, 17, "ProceedWithCarrier",
                Item.L(
                    Item.U4(2),
                    Item.A("ProceedWithCarrier"),
                    Item.A(tx.Carrier.Id),
                    Item.B(GetPortNo(tx.Carrier.LoadPortId)),
                    Item.L())));

            byte returnCode = (byte)s3f18.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("ProceedWithCarrier failed : " + s3f18.SecsItem.Items[1].Items[0].Items[1]);
        }
    }
}