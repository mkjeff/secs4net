using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Threading.Tasks;
using static Secs4Net.Item;

namespace Cim.Eap {
    partial class Driver {
        async Task TCS_ToolModeChange(ToolModeChangeRequest tx)
        {
            if (tx.Mode == ControlMode.Online)
            {
                await TestCommunication();
                await RequestOnline();
                await QueryControlState();
                await DefineLink();
                await QueryAndChangeAccessMode(tx.LoadPorts);

                await Task.Delay(2000);
                await QueryPortStatus();
            }
            else
            {
                await RequestOffline(tx);
            }
        }

        async Task TestCommunication() {
            using (var s1f14 = await EAP.SendAsync(EAP.SecsMessages[1, 13, "EstablishCommunicationsRequest_Host"], false))
                if ((byte) s1f14.SecsItem.Items[0] != 0)
                    throw new ScenarioException("S1F14 return code is not 0");
        }


        async Task RequestOnline() {
            using (var s1f18 = await EAP.SendAsync(EAP.SecsMessages[1, 17, "RequestOnline"], false))
            {
                var returnCode = (byte) s1f18.SecsItem;
                if (returnCode != 0 && returnCode != 2)
                    throw new ScenarioException("S1F18 return code is " + returnCode);
            }
        }

        async Task QueryControlState() {
            using (var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryOnlineSubStatus"], false))
                if ((ushort)s1f4.SecsItem.Items[0] != 5)
                    throw new ScenarioException("S1F4_ControlState return code is " + s1f4.SecsItem.Items[0] + ", not Online/Remote mode");
        }

        async Task QueryPortStatus()
        {
            using (await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryPortStatus"], false))
            {
            }
        }

        async Task RequestOffline(ToolModeChangeRequest request) {
            try
            {
                foreach (var port in request.LoadPorts)
                    using (await EAP.SendAsync(CreateS3F23(port)))
                    {
                    }
            }
            catch {}
            try{
                using (await EAP.SendAsync(EAP.SecsMessages[1, 19, "RequestOffline"]))
                {
                }
            } catch { }
        }

        private static SecsMessage CreateS3F23(LoadPort port)
            => new SecsMessage(3, 23, "ChangeAccessMode",
                   L(
                       A("ChangeAccess"),
                       A(GetPortNo(port.Id).ToString()),
                       L(
                           L(
                               A("AccessMode"),
                               B(0)))));
    }
}