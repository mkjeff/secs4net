using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct CarrierOutCompleteReport : ITxReport {
        public string UnloadPortID { get; set; }
        public string CarrierId { get; set; }
        public string BufferResources { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="CarrierOutCompleteReport" Type="Event" MessageKey="2535">
        //  <Tool ToolID="FHT04" UnloadPortID="L01" CarrierID="BSW04172" BufferResources=""/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "CarrierOutCompleteReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("UnloadPortID", UnloadPortID ?? string.Empty),
        new XAttribute("CarrierID", CarrierId ?? string.Empty),
        new XAttribute("BufferResources", BufferResources ?? string.Empty)));
    }
}
