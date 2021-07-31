using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
	public struct AccessModeChangeRequest : ITxMessage {
		public IEnumerable<LoadPort> LoadPorts { get; private set; }

        //<?xml version="1.0" ?>  
        //<Transaction TxName="AccessModeChange" Type="Request" MessageKey="1399">
        //  <Tool ToolID="MAU01" EquipmentOnlineMode="On-Line Remote" />
        //  <LoadPorts>
        //      <LoadPort LoadPortID="L02" EquipmentAccessMode="Manual" LoadPurposeType="Process Lot"/>
        //  </LoadPorts>
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            LoadPorts = from portElm in txElm.Element("LoadPorts").Elements("LoadPort")
                        select new LoadPort {
                            Id = portElm.Attribute("LoadPortID").Value,
                            AccessMode = portElm.Attribute("EquipmentAccessMode").Value.ToEnum<AccessMode>()
                        };
        }
	}
}