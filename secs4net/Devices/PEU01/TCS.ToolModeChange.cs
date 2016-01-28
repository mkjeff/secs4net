using System.Threading;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
using System.Linq;
namespace Cim.Eap {
    partial class Driver {
        void HandleTCS(ToolModeChangeRequest tx) {
            if (tx.Mode == ControlMode.Online) {
                CommunicationTest();
                RequestOnline();
                QueryControlState();
                DefineLink();
                QueryAndChangeAccessMode(tx.LoadPorts);
                ThreadPool.QueueUserWorkItem(_ => {
                    Thread.Sleep(2000);
                    QueryPortState();
                });
            } else {
                RequestOffLine(tx);
            }
        }

        private void CommunicationTest() {
            var s1f14 = EAP.Send(EAP.SecsMessages[1, 13, "EstablishCommunicationsRequest_Host"]);

            if ((byte)s1f14.SecsItem.Items[0] != 0)
                throw new ScenarioException("S1F14 return code is not 0");
        }

        private void RequestOnline() {
            var s1f18 = EAP.Send(EAP.SecsMessages[1, 17, "RequestOnLine"]);
            byte returnCode = (byte)s1f18.SecsItem;
            if (returnCode != 0 && returnCode != 2)
                throw new ScenarioException("S1F18 return code is " + returnCode);
        }

        private void QueryControlState() {
            var s1f4 = EAP.Send(EAP.SecsMessages[1, 3, "QueryOnlineSubStatus"]);
            byte returnCode = (byte)s1f4.SecsItem.Items[0];
            if (returnCode != 5)
                throw new ScenarioException("S1F4_ControlState return code is " + returnCode + ", not Online/Remote mode");
        }

        private void QueryPortState() {
            var s1f4 = EAP.Send(EAP.SecsMessages[1, 3, "QueryPortStatus"]);
            for (int i = 0; i < s1f4.SecsItem.Items.Count; i++) {
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

		private void RequestOffLine(ToolModeChangeRequest request) {
			try {
                foreach (var port in request.LoadPorts)
                    EAP.Send(new SecsMessage(3, 23, "ChangeAccessMode",
                        Item.L(
                            Item.A(),
                            Item.U1(0),
                            Item.L(
                                Item.U1(GetPortNo(port.Id))))));
            }catch{}
            try{
                EAP.Send(EAP.SecsMessages[1, 15, "RequestOffLine"]);
			} catch{
			}
		}
	}
}