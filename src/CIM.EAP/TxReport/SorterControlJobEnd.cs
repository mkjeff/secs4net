using System.Xml.Linq;
using Cim.Eap.Data;
using Cim.Eap.Tx;

namespace Cim.Eap.Tx {
    public struct SorterControlJobEndReport:ITxReport {
        public string ControlJobID;
        public string ActionCode;

        public Carrier SourceMap;
        public Carrier DestinationMap;

        //<Transaction TxName="SorterControlJobEndReport" Type="Event" MessageKey="5981">
        //  <Tool ToolID="XM301" />
        //  <ControlJob ControlJobID="XM301-20100421-0002B" />
        //  <SourceCarrier SourceCarrierCnt="1" />
        //  <ActionCode Code="Host\Read" />
        //  <OnRoute Flag="True" />
        //  <SourceMap>
        //    <Carriers>
        //      <Carrier CarrierID="ASR07673" LoadPurposeType="" LoadPortID="">
        //        <SlotMap>
        //          <Slot SlotNo="1">BET746-01</Slot>
        //          <Slot SlotNo="8">BET746-08</Slot>
        //          <Slot SlotNo="25">BET746-25</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </SourceMap>
        //  <DestinationMap>
        //    <Carriers>
        //    </Carriers>
        //  </DestinationMap>
        //</Transaction>
        XElement ITxReport.XML => new XElement("Transaction",
    new XAttribute("TxName", "SorterControlJobStartReport"),
    new XAttribute("Type", "Event"),
    new XElement("Tool", new XAttribute("ToolID", string.Empty)),
    new XElement("ControlJob", new XAttribute("ControlJobID", ControlJobID ?? string.Empty)),
    new XElement("SourceCarrier", new XAttribute("SourceCarrierCnt", 1)),
    new XElement("ActionCode", new XAttribute("Code", "?")),
    new XElement("OnRoute", new XAttribute("Flag", true)),
    new XElement("SourceMap",
        new XElement("Carriers")),
    new XElement("DestinationMap",
        new XElement("Carriers")));
    }
}
