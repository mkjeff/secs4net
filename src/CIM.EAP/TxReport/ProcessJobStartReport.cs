using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ProcessJobStartReport : ITxReport {
        public string ProcessJobID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ProcessJobEndReport" Type="Event" MessageKey="2473">
        //  <Tool ToolID="FHT04"/>
        //  <ProcessJob ProcessJobID="AGE153.010"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ProcessJobStartReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ProcessJob", new XAttribute("ProcessJobID", ProcessJobID ?? string.Empty)));
    }
}
