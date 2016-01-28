using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ControlJobStartReport : ITxReport {
        public string ControlJobID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ControlJobStartReport" Type="Event" MessageKey="2502">
        //  <Tool ToolID="FHT04"/>
        //  <ControlJob ControlJobID="FHT04-20100301-0001A"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ControlJobStartReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ControlJob", new XAttribute("ControlJobID", ControlJobID ?? string.Empty)));
    }
}
