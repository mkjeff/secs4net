using System.Xml.Linq;
namespace Cim.Eap.Tx {
	public struct CarrierCompleteReport : ITxReport {
		public string CarrierId { get; set; }

        // <Transaction TxName="CarrierCompleteReport" Type="Event" MessageKey="7003">
        //		<Tool ToolID="FNT04" CarrierID="BSW13642"/>
        // </Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "CarrierCompleteReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("CarrierID", CarrierId ?? string.Empty)));
    }
}