using System.Xml.Linq;
namespace Cim.Eap.Tx {

    public struct CarrierInCompleteReport : ITxReport {
        public string LoadPortID { get; set; }
        public string CarrierId { get; set; }
        public string BufferResources { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="CarrierInCompleteReport" Type="Event" MessageKey="2501">
        //  <Tool ToolID="FHT04" LoadPortID="L01" CarrierID="BSW64082" BufferResources="Extra Dummy Lot"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "CarrierInCompleteReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("LoadPortID", LoadPortID ?? string.Empty),
        new XAttribute("CarrierID", CarrierId ?? string.Empty),
        new XAttribute("BufferResources", BufferResources ?? string.Empty)));
    }

}