using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
    public struct ProceedWithCarrierRequest : ITxMessage {
        public Carrier Carrier { get; private set; }

        //<?xml version="1.0" ?>  
        //<Transaction TxName="ProceedWithCarrier" Type="Request" MessageKey="1212"> 
        //	<Tool ToolID="MAU01" /> 
        //	<Carriers>
        //		<Carrier CarrierID="ASR10483" LoadPurposeType="Process Lot" LoadPortID="L02">
        //			<SlotMap>
        //				<Slot SlotNo="1">BEP527-01</Slot>
        //				<Slot SlotNo="2">BEP527-02</Slot>
        //				<Slot SlotNo="24">BEP527-24</Slot>
        //				<Slot SlotNo="25">BEP527-25</Slot>
        //			</SlotMap> 
        //		</Carrier> 
        //	</Carriers> 
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            XElement carierElm = txElm.Element("Carriers").Element("Carrier");
            Carrier = new Carrier {
                Id = carierElm.Attribute("CarrierID").Value,
                LoadPurposeType = carierElm.Attribute("LoadPurposeType").Value,
                LoadPortId = carierElm.Attribute("LoadPortID").Value,
                SlotMap = from slotElm in carierElm.Element("SlotMap").Elements("Slot")
                          select new SlotInfo {
                              SlotNo = (byte)(int)slotElm.Attribute("SlotNo"),
                              WaferID = slotElm.Value
                          }
            };
        }
    }
}
