using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ProcessJobEndReport : ITxReport {
        public string ProcessJobID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ProcessJobStartReport" Type="Event" MessageKey="2568">
        //  <Tool ToolID="FHT04"/>
        //  <ProcessJob ProcessJobID="AGK672.000"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ProcessJobEndReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ProcessJob", new XAttribute("ProcessJobID", ProcessJobID ?? string.Empty)));
    }
}