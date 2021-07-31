using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
    public struct CreateSorterControlJobRequest : ITxMessage {
        public SorterJob JobInfo { get; }

        //<Transaction TxName="CreateSorterControlJob" Type="Request" MessageKey="6663">
        //  <Tool ToolID="XM301" />
        //  <ControlJob ControlJobID="XM301-20100421005707-0500" />
        //  <SourceCarrier SourceCarrierCnt="1" />
        //  <ActionCode Code="Host\Transfer" />
        //  <OnRoute Flag="FALSE" />
        //  <SourceMap>
        //    <Carriers>
        //      <Carrier CarrierID="ASR02653" LoadPurposeType="Process Lot" LoadPortID="L03">
        //        <SlotMap>
        //          <Slot SlotNo="1">BFB042-01</Slot>
        //          <Slot SlotNo="20">BFB042-20</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </SourceMap>
        //  <DestinationMap>
        //    <Carriers>
        //      <Carrier CarrierID="ATR04873" LoadPurposeType="Process Lot" LoadPortID="L04">
        //        <SlotMap>
        //          <Slot SlotNo="1">BFB042-01</Slot>
        //          <Slot SlotNo="20">BFB042-20</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </DestinationMap>
        //  <StartMethod StartMethodID="AUTO" />
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
        }
	}
}