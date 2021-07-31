using System.Xml.Linq;
namespace Cim.Eap.Tx {
	public struct ProcessedWaferCountReport : ITxReport {
		public string ProcessJobID { get; set; }
		public byte WaferCount { get; set; }

        XElement ITxReport.XML => new XElement("Transaction",
            new XAttribute("TxName", "ProcessedWaferCountReport"),
            new XAttribute("Type", "Event"),
            new XElement("Tool", new XAttribute("ToolID", string.Empty)),
            new XElement("ProcessJob",
                new XAttribute("ProcessJobID", ProcessJobID ?? string.Empty),
                new XAttribute("WaferCount", WaferCount)));
    }
}