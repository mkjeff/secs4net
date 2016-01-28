using System.Xml.Linq;
namespace Cim.Eap.Tx {
    public struct ControlJobEndReport : ITxReport {
        public string ControlJobID { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ControlJobEndReport" Type="Event" MessageKey="2478">
        //  <Tool ToolID="FHT04"/>
        //  <ControlJob ControlJobID="FHT04-20100228-0003A"/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ControlJobEndReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ControlJob", new XAttribute("ControlJobID", ControlJobID ?? string.Empty)));
    }
}
