using System.Xml.Linq;

namespace Cim.Eap.Tx {
    public struct WaferStartReport:ITxReport {
        public byte SlotNo { get; set; }
        public string ProcessJobId { get; set; }
        public string CarrierId { get; set; }

        //<?xml version="1.0"?>
        //<Transaction TxName="WaferStartReport" Type="Event" MessageKey="4812">
        //  <Tool ToolID="PEU01" CarrierID="AVR23732" ProcessJobID="AGH913.030" SlotNo="02" />
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "WaferStartReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("ProcessJobID", ProcessJobId ?? string.Empty),
        new XAttribute("CarrierID", CarrierId ?? string.Empty),
        new XAttribute("SlotNo", SlotNo)));
    }
}
