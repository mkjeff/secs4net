using System.Linq;
using Secs4Net;
using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {
        void EQP_UnloadComplete(SecsMessage msg) {
            string portId = GetPortID((byte)msg.SecsItem.Items[2].Items[0].Items[1].Items[0]);
        }
    }
}