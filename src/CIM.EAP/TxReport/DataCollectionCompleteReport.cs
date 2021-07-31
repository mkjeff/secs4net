using System.Xml.Linq;
using Cim.Eap.Data;

namespace Cim.Eap.Tx {
	public struct DataCollectionCompleteReport : ITxReport {
        public readonly ProcessJob ProcessJob;
        public DataCollectionCompleteReport(ProcessJob processJob) {
            ProcessJob = processJob;
        }

        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "DataCollectionCompleteReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool",
        new XAttribute("ToolID", string.Empty),
        new XAttribute("LoadPortID", string.Empty),
        new XAttribute("ProcessJobID", ProcessJob.Id)));
    }
}