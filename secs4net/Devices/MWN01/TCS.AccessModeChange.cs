using System.Collections.Generic;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using Cim.Eap;

namespace Eap.Driver.MWN {
    partial class Driver {
        Task TCS_AccessModeChange(AccessModeChangeRequest tx)
        {
            return QueryAndChangeAccessMode(tx.LoadPorts);
        }

        async Task QueryAndChangeAccessMode(IEnumerable<LoadPort> requestLoadPorts) {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryLoadPortAccessMode"]);
            foreach (LoadPort port in requestLoadPorts) {
                byte portNo = GetPortNo(port.Id);
                // 0: Manual
                // 1: Auto
                if ((byte)s1f4.SecsItem.Items[portNo - 1] != (byte)port.AccessMode)
                    await ChangeAccessMode(port.AccessMode, portNo);
            }
        }

        async Task ChangeAccessMode(AccessMode changeAccessMode, int portNo) {
            var s3f24 = await EAP.SendAsync(new SecsMessage(3, 23, "ChangeAccessMode",
                Item.L(
                    Item.A("ChangeAccess"),
                    Item.A(portNo.ToString()),
                    Item.L(
                        Item.L(
                            Item.A("AccessMode"),
                            Item.B((byte)changeAccessMode))))));
            byte returnCode = (byte)s3f24.SecsItem.Items[0];
            if (returnCode != 0 && returnCode != 4)
                throw new ScenarioException("Change Loadport[" + portNo + "] access mode fial: " + s3f24.SecsItem.Items[1].Items[0].Items[1].GetString());
        }
    }
}