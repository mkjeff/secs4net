using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct UnloadCompleteReport : ITxReport {
        public string PortID { get; set; }
        public string CarrierID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="UnloadCompleteReport" Type="Event" MessageKey="2542">
        //  <Tool ToolID="FHT04" UnloadPortID="L01" CarrierID="BSW04172" AccessMode=""/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "UnloadCompleteReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("UnloadPortID", PortID ?? string.Empty),
        new XAttribute("CarrierID", CarrierID ?? string.Empty),
        new XAttribute("AccessMode", string.Empty)));
    }
}