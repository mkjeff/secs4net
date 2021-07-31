using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
    public struct CreateSorterProcessJobRequest : ITxMessage {
        public SorterJob JobInfo { get; }

        //<?xml version="1.0"?>
        //<Transaction TxName="CreateSorterProcessJob" Type="Request" MessageKey="6536">
        //  <Tool ToolID="XM301" />
        //  <ControlJob ControlJobID="XM301-20100421-0001A" />
        //  <SourceCarrier SourceCarrierCnt="1" /> 
        //  <ActionCode Code="Host\Transfer" />
        //  <OnRoute Flag="TRUE" /> 
        //  <SourceMap>
        //    <Carriers>
        //      <Carrier CarrierID="BSW20762" LoadPurposeType="Process Lot" LoadPortID="L03">
        //        <SlotMap>
        //          <Slot SlotNo="7">12A-909090420</Slot>
        //          <Slot SlotNo="9">12A-K9NDG024TM-PS-R0</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </SourceMap>
        //  <DestinationMap>
        //    <Carriers>
        //      <Carrier CarrierID="BSW78532" LoadPurposeType="Empty Cassette" LoadPortID="L04">
        //        <SlotMap>
        //          <Slot SlotNo="7">12A-909090420</Slot>
        //          <Slot SlotNo="9">12A-K9NDG024TM-PS-R0</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </DestinationMap>
        //  <StartMethod StartMethodID="AUTO" />
        //</Transaction>

        //<Transaction TxName="CreateSorterProcessJob" Type="Request" MessageKey="6649">
        //  <Tool ToolID="XM301" />
        //  <ControlJob ControlJobID="XM301-20100421-0002B" />
        //  <SourceCarrier SourceCarrierCnt="1" />
        //  <ActionCode Code="Host\Read" />
        //  <OnRoute Flag="TRUE" />
        //  <RecipeParameters>
        //    <RecipeParameter Name="WaferPostAlignMap">180</RecipeParameter>
        //  </RecipeParameters>
        //  <SourceMap>
        //    <Carriers>
        //      <Carrier CarrierID="ASR07673" LoadPurposeType="Process Lot" LoadPortID="L03">
        //        <SlotMap>
        //          <Slot SlotNo="1">BET746-01</Slot>
        //          <Slot SlotNo="25">BET746-25</Slot>
        //        </SlotMap>
        //      </Carrier>
        //    </Carriers>
        //  </SourceMap>
        //  <StartMethod StartMethodID="AUTO" />
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
        }
    }
}