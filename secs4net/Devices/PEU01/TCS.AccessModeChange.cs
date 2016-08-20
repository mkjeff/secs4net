﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        async void HandleTCS(AccessModeChangeRequest tx) {
            QueryAndChangeAccessMode(tx.LoadPorts);
        }

        async Task QueryAndChangeAccessMode(IEnumerable<LoadPort> requestLoadPorts) {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryLoadPortAccessMode"]);
            foreach (var port in requestLoadPorts) {
                byte portNo = GetPortNo(port.Id);
                // 0: Manual
                // 1: Auto
                byte portAccessMode = (byte)s1f4.SecsItem.Items[portNo - 1];
                if (portAccessMode != (byte)port.AccessMode)
                    await ChangeAccessMode(port.AccessMode, portNo);
            }
        }

        async Task ChangeAccessMode(AccessMode changeAccessMode, byte portNo) {
            var S3F23 = new SecsMessage(3, 23, "ChangeAccessMode",
                Item.L(
                    Item.A(),
                    Item.U1((byte)changeAccessMode),
                    Item.L(
                        Item.U1(portNo))));

            var S3F24 = await EAP.SendAsync(S3F23);
            byte returnCode = (byte)S3F24.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("Change Loadport[" + portNo + "] access mode fial. " + S3F24.SecsItem.Items[1].Items[1].ToString());
        }
    }
}