using System.Xml.Linq;
using Cim.Eap.Tx;

namespace Cim.Eap.Tx {
    public struct SorterControlJobStartReport : ITxReport {
        public string ControlJobID { get; set; }

        //<Transaction TxName="SorterControlJobStartReport" Type="Event" MessageKey="5930">
        //  <Tool ToolID="XM301" />
        //  <ControlJob ControlJobID="XM301-20100421-0002B" />
        //  <OnRoute Flag="False" />
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "SorterControlJobStartReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ControlJob", new XAttribute("ControlJobID", ControlJobID ?? string.Empty)));
    }
}
