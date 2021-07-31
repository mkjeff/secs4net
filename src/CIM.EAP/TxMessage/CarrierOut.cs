using System.Xml.Linq;
using Cim.Eap.Data;

namespace Cim.Eap.Tx {
    public struct CarrierOutRequest : ITxMessage {
        public Carrier Carrier { get; private set; }

        //<?xml version="1.0" ?>  
        //<Transaction TxName="CarrierOut" Type="Request" MessageKey="3341"> 
        //  <Tool ToolID="FHT04" /> 
        //  <Carriers>
        //      <Carrier CarrierID="BSW50392" UnloadPortID="L01"></Carrier> 
        //  </Carriers> 
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            XElement carierElm = txElm.Element("Carriers").Element("Carrier");
            Carrier = new Carrier {
                Id = carierElm.Attribute("CarrierID").Value,
                LoadPortId = carierElm.Attribute("UnloadPortID").Value
            };
        }
    }
}