using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ChamberStatusReport : ITxReport {
        public string ChamberID { get; set; }
        public string ChamberStatus { get; set; }

        //<Transaction TxName="ChamberStatusReport" Type="Event" MessageKey="7130">
        //  <Tool ToolID="CIE04" ChamberID="L" ChamberStatus="0000"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ChamberStatusReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty),
        new XAttribute("ChamberID", ChamberID ?? string.Empty),
        new XAttribute("ChamberStatus", ChamberStatus ?? string.Empty)));
    }
}


