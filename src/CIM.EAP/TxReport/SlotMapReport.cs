using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
namespace Cim.Eap.Tx {
	public struct SlotMapReport : ITxReport {
		public string LoadPortID { get; set; }
		public string CarrierID { get; set; }
        public IEnumerable<byte> Slots { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="SlotMapReport" Type="Event" MessageKey="2499">
        //  <Tool ToolID="FHT04" LoadPortID="L01" CarrierID="BSW64082"/>
        //  <SlotMap>
        //      <Slot SlotNo="1"/>
        //      <Slot SlotNo="2"/>
        //      <Slot SlotNo="24"/>
        //      <Slot SlotNo="25"/>
        //  </SlotMap>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "SlotMapReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("LoadPortID", LoadPortID ?? string.Empty),
        new XAttribute("CarrierID", CarrierID ?? string.Empty)),
    new XElement("SlotMap",
        from slot in Slots
        select new XElement("Slot", new XAttribute("SlotNo", slot))));
    }
}