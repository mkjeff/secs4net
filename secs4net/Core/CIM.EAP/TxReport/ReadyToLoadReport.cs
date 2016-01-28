using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ReadyToLoadReport : ITxReport {
        public string PortID { get; set; }
        public string CarrierID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ReadyToLoadReport" Type="Event" MessageKey="2543">
        //  <Tool ToolID="FHT04" LoadPortID="L01" CarrierID="" AccessMode=""/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ReadyToLoadReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("LoadPortID", PortID ?? string.Empty),
        new XAttribute("CarrierID", CarrierID ?? string.Empty),
        new XAttribute("AccessMode", string.Empty)));
    }
}
