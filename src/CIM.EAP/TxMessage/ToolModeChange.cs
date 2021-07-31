using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
	public struct ToolModeChangeRequest : ITxMessage {
		public ControlMode Mode { get; private set; }
		public IEnumerable<LoadPort> LoadPorts { get; private set; }

        void ITxMessage.Parse(XElement txElm) {
            Mode = txElm.Element("Tool").Attribute("EquipmentOnlineMode").Value == "Off-Line" ? ControlMode.Offline : ControlMode.Online;
            LoadPorts = from portElm in txElm.Element("LoadPorts").Elements("LoadPort")
                        select new LoadPort {
                            Id = portElm.Attribute("LoadPortID").Value,
                            AccessMode = portElm.Attribute("EquipmentAccessMode").Value.ToEnum<AccessMode>()
                        };
        }
	}
}