using System.Xml.Linq;
namespace Cim.Eap.Tx {
	public struct ControlJobsInfoInquery : ITxReport {
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ControlJobsInfoInquery"),
    new XAttribute("Type", "Request"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)));
    }
}