using System.Xml.Linq;
using Cim.Eap.Data;

namespace Cim.Eap.Tx {
    public struct CancelCarrierRequest : ITxMessage {
        public Carrier Carrier { get; private set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="CancelCarrier" Type="Request" MessageKey="1397" >
        //  <Tool ToolID="MAU01" />
        //  <Carriers>
        //      <Carrier CarrierID="ASR02623" LoadPurposeType="Process Lot" LoadPortID="L01" >
        //          <SlotMap></SlotMap>
        //      </Carrier>
        //  </Carriers>
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            XElement carierElm = txElm.Element("Carriers").Element("Carrier");
            Carrier = new Carrier {
                Id = carierElm.Attribute("CarrierID").Value,
                LoadPurposeType = carierElm.Attribute("LoadPurposeType").Value,
                LoadPortId = carierElm.Attribute("LoadPortID").Value,
            };
        }
    }
}
