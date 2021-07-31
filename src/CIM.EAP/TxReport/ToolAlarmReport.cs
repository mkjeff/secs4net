using System.Xml.Linq;
using System;

namespace Cim.Eap.Tx {
	public struct ToolAlarmReport : ITxReport {
		public string AlarmCode { get; set; }
		public string AlarmId { get; set; }
		public string AlarmText { get; set; }

        //<?xml version="1.0" ?> 
        //<Transaction TxName="ToolAlarmReport" Type="Event" MessageKey="2517">
        //  <Tool ToolID="FHT04" TimeStamp="2010/03/16 23:57:08.29" />
        //  <Alarm AlarmCode="134" AlarmID="16387" AlarmCategory="S5F1" AlarmText="D Over Use Limit(Caution)               "/>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "ToolAlarmReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("TimeStamp", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff"))),
    new XElement("Alarm",
        new XAttribute("AlarmCode", AlarmCode ?? string.Empty),
        new XAttribute("AlarmID", AlarmId ?? string.Empty),
        new XAttribute("AlarmCategory", "S5F1"),
        new XAttribute("AlarmText", AlarmText ?? string.Empty)));
    }
}