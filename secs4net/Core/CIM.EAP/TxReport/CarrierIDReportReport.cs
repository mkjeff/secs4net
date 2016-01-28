using System.Xml.Linq;
namespace Cim.Eap.Tx {
	public struct CarrierIDReport : ITxReport {
		public string LoadPortId { get; set; }
		public string CarrierId { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="CarrierIDReport" Type="Event" MessageKey="2498">
        //  <Tool ToolID="FHT04" LoadPortID="L01" CarrierID="BSW64082" AccessMode=""/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "CarrierIDReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("LoadPortID", LoadPortId ?? string.Empty),
        new XAttribute("CarrierID", CarrierId ?? string.Empty),
        new XAttribute("AccessMode", string.Empty)));
    }
}