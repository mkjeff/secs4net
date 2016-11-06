using System.Collections.Generic;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        Task TCS_AccessModeChange(AccessModeChangeRequest tx) => QueryAndChangeAccessMode(tx.LoadPorts);

        async Task QueryAndChangeAccessMode(IEnumerable<LoadPort> requestLoadPorts) {
            using (var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryLoadPortAccessMode"], false))
            {
                foreach (var port in requestLoadPorts)
                {
                    byte portNo = GetPortNo(port.Id);
                    // 0: Manual
                    // 1: Auto
                    if ((byte) s1f4.SecsItem.Items[portNo - 1] != (byte) port.AccessMode)
                        await ChangeAccessMode(port.AccessMode, portNo);
                }
            }
        }

        async Task ChangeAccessMode(AccessMode changeAccessMode, int portNo)
        {
            using (var s3f24 = await EAP.SendAsync(CreateS3F23(changeAccessMode, portNo)))
            {
                byte returnCode = (byte) s3f24.SecsItem.Items[0];
                if (returnCode != 0 && returnCode != 4)
                    throw new ScenarioException($"Change Loadport[{portNo}] access mode fial: {s3f24.SecsItem.Items[1].Items[0].Items[1].GetString()}");
            }
        }

        private static SecsMessage CreateS3F23(AccessMode changeAccessMode, int portNo)
            => new SecsMessage(3, 23, "ChangeAccessMode",
                   L(
                       A("ChangeAccess"),
                       A(portNo.ToString()),
                       L(
                           L(
                               A("AccessMode"),
                               B((byte) changeAccessMode)))));
    }
}