using System.Linq;
using Secs4Net;
using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void EQP_UnloadComplete(SecsMessage msg) {
            var portId = GetPortID((byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0]);
            EAP.Report(new UnloadCompleteReport {
                PortID = portId
            });

            //清除相關Loadport上的ProcessJob物件
            _ProcessingJobs.RemoveAll(pj => pj.Carriers.First().LoadPortId == portId);
        }
    }
}