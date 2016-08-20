﻿using System.Threading;
using System.Threading.Tasks;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using Secs4Net;
namespace Cim.Eap {
    partial class Driver {
        async void TCS_ToolModeChange(ToolModeChangeRequest tx) {
            if (tx.Mode == ControlMode.Online) {
                TestCommunication();
                RequestOnline();
                QueryControlState();
                DefineLink();
                QueryAndChangeAccessMode(tx.LoadPorts);

                ThreadPool.QueueUserWorkItem(delegate {
                    Thread.Sleep(2000);
                    QueryPortStatus();
                });
            } else {
                RequestOffline(tx);
            }
        }

        async Task TestCommunication() {
            var s1f14 = await EAP.SendAsync(EAP.SecsMessages[1, 13, "EstablishCommunicationsRequest_Host"]);
            if ((byte)s1f14.SecsItem.Items[0] != 0)
                throw new ScenarioException("S1F14 return code is not 0");
        }


        async Task RequestOnline() {
            var s1f18 = await EAP.SendAsync(EAP.SecsMessages[1, 17, "RequestOnline"]);
            byte returnCode = (byte)s1f18.SecsItem;
            if (returnCode != 0 && returnCode != 2)
                throw new ScenarioException("S1F18 return code is " + returnCode);
        }

        async Task QueryControlState() {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryOnlineSubStatus"]);
            if ((ushort)s1f4.SecsItem.Items[0] != 5)
                throw new ScenarioException("S1F4_ControlState return code is " + s1f4.SecsItem.Items[0] + ", not Online/Remote mode");
        }

        async Task QueryPortStatus() {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "QueryPortStatus"]);
            for (int i = 0; i < s1f4.SecsItem.Items.Count; i++) {
                switch ((byte)s1f4.SecsItem.Items[i]) {
                    case 2:
                        EAP.Report(new ReadyToLoadReport { PortID = GetPortID(i + 1) });
                        break;
                    case 3:
                        EAP.Report(new ReadyToUnloadReport { PortID = GetPortID(i + 1) });
                        break;
                }
            }
        }

        async Task RequestOffline(ToolModeChangeRequest request) {
            try {
                foreach (var port in request.LoadPorts)
                    await EAP.SendAsync(new SecsMessage(3, 23, "ChangeAccessMode",
                        Item.L(
                            Item.A("ChangeAccess"),
                            Item.A(GetPortNo(port.Id).ToString()),
                            Item.L(
                                Item.L(
                                    Item.A("AccessMode"),
                                    Item.B(0))))));
            }catch{}
            try{
                await EAP.SendAsync(EAP.SecsMessages[1, 19, "RequestOffline"]);
            } catch { }
        }
    }
}