using System.Threading;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Linq;
using System.Threading.Tasks;

namespace Cim.Eap {
    partial class Driver {
        async Task HandleTCS(ToolModeChangeRequest tx)
        {
            if (tx.Mode == ControlMode.Online)
            {
                await CommunicationTest();
                await RequestOnline();
                await QueryControlState();
                await DefineLink();
                await QueryAndChangeAccessMode(tx.LoadPorts);
                await Task.Delay(2000);
                await QueryPortState();
            }
            else {
                await RequestOffLine(tx);
            }
        }

        async Task CommunicationTest() {
            var s1f14 = await EAP.SendAsync(EAP.SecsMessages[1, 13, "EstablishCommunicationsRequest_Host"]);

            if ((byte)s1f14.SecsItem.Items[0] != 0)
                throw new ScenarioException("S1F14 return code is not 0");
        }

        async Task RequestOnline() {
            var s1f18 = await EAP.SendAsync(EAP.SecsMessages[1, 17, "RequestOnLine"]);
            var returnCode = (byte)s1f18.SecsItem;
            if (returnCode != 0 && returnCode != 2)
                throw new ScenarioException("S1F18 return code is " + returnCode);
        }

        async Task QueryControlState() {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryOnlineSubStatus"]);
            var returnCode = (byte)s1f4.SecsItem.Items[0];
            if (returnCode != 5)
                throw new ScenarioException("S1F4_ControlState return code is " + returnCode + ", not Online/Remote mode");
        }

        async Task QueryPortState() {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryPortStatus"]);
            for (var i = 0; i < s1f4.SecsItem.Items.Count; i++) {
                switch ((byte)s1f4.SecsItem.Items[i]) {
                    case 1:
                        EAP.Report(new ReadyToLoadReport { PortID = GetPortID(i + 1) });
                        break;
                    case 3:
                        EAP.Report(new ReadyToUnloadReport { PortID = GetPortID(i + 1) });
                        break;
                }
            }
        }

        async Task RequestOffLine(ToolModeChangeRequest request) {
			try {
                foreach (var port in request.LoadPorts)
                    await EAP.SendAsync(new SecsMessage(3, 23, "ChangeAccessMode",
                        Item.L(
                            Item.A(),
                            Item.U1(0),
                            Item.L(
                                Item.U1(GetPortNo(port.Id))))));
            }catch{}
            try{
                await EAP.SendAsync(EAP.SecsMessages[1, 15, "RequestOffLine"]);
			} catch{
			}
		}
	}
}