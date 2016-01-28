using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
namespace Cim.Eap.Tx {
	public struct ToolInventoryReport : ITxReport {
        public IEnumerable<string> Carriers { get; set; }

        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ToolInventoryReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("Carriers",
        from carrier in Carriers
        select new XElement("Carrier", new XAttribute("CarrierID", carrier))));
    }
}