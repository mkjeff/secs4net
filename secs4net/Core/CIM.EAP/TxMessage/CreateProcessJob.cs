using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
	public struct CreateProcessJobRequest : ITxMessage {
		public IEnumerable<ProcessJob> ProcessJobs { get; private set; }

        //<?xml version="1.0"?>
        //<Transaction TxName="CreateProcessJob" Type="Request" MessageKey="5547">
        //    <Tool ToolID="QXN03"/>
        //    <ProcessJobIDs>
        //        <ProcessJob ProcessJobID="AGK547.000">
        //            <Material Type="SameLot"/>
        //            <Recipe RecipeID="DL01ESMT/BS110DL01"/>
        //            <RecipeParameters>
        //                <RecipeParameter Name="MW">2</RecipeParameter>
        //            </RecipeParameters>
        //            <Reticle ReticleID=""/>
        //            <Carriers>
        //                <Carrier CarrierID="BSW77492" LoadPurposeType="Process Lot" LoadPortID="L02">
        //                    <SlotMap>
        //                        <Slot SlotNo="1">AGK547-01</Slot>
        //                        <Slot SlotNo="2">AGK547-02</Slot>
        //                        <Slot SlotNo="3">AGK547-03</Slot>
        //                        <Slot SlotNo="23">AGK547-23</Slot>
        //                        <Slot SlotNo="25">AGK547-25</Slot>
        //                    </SlotMap>
        //                    <MeasurementSlots>
        //                        <MeasurementSlot SlotNo="1"/>
        //                        <MeasurementSlot SlotNo="10"/>
        //                    </MeasurementSlots>
        //                </Carrier>
        //            </Carriers>
        //            <DataItems DataCollectionDefinitionID="QXN01.00"/>
        //            <StartMethod StartMethodID="AUTO"/>
        //            <Lots>
        //                <Lot LotID="AGK547.000" LotType="Production" CarrierID="BSW77492" ProductID="AAM297A1B.0E01" OperationNo="BS.PQX10" FlowBatchID="" PassCount="1" Fab="12A" RouteID="CFDL0101.AA"/>
        //            </Lots>
        //        </ProcessJob>
        //    </ProcessJobIDs>
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            this.ProcessJobs = (from pjElm in txElm.Element("ProcessJobIDs").Elements("ProcessJob")
                                select new ProcessJob(
                                    pjElm.Attribute("ProcessJobID").Value,
                                    pjElm.Element("Recipe").Attribute("RecipeID").Value,
                                    pjElm.Element("Reticle").Attribute("ReticleID").Value,
                                    pjElm.Element("DataItems").Attribute("DataCollectionDefinitionID").Value,
                                    pjElm.Element("StartMethod").Attribute("StartMethodID").Value,
                                    from carrierElm in pjElm.Element("Carriers").Elements("Carrier")
                                    select CreateCarrierFromElm(carrierElm),
                                    from paramElm in pjElm.Element("RecipeParameters").Elements("RecipeParameter")
                                    select new RecipeParameter {
                                        Name = paramElm.Attribute("Name").Value,
                                        Value = paramElm.Value
                                    },
                                    from lotElm in pjElm.Element("Lots").Elements("Lot")
                                    select new EDALotInfo {
                                        LotID = lotElm.Attribute("LotID").Value,
                                        ProductID = lotElm.Attribute("ProductID").Value,
                                        OperationNo = lotElm.Attribute("OperationNo").Value,
                                        PassCount = lotElm.Attribute("PassCount").Value,
                                        FlowBatchID = lotElm.Attribute("FlowBatchID").Value,
                                        Fab = lotElm.Attribute("Fab").Value,
                                        CarrierID = lotElm.Attribute("CarrierID").Value,
                                        RouteID = lotElm.Attribute("RouteID").Value
                                    })).ToList();
        }

        static Carrier CreateCarrierFromElm(XElement carrierElm) {
            string carrierId = carrierElm.Attribute("CarrierID").Value;
            string loadPurposeType = carrierElm.Attribute("LoadPurposeType").Value;
            string loadPortId = carrierElm.Attribute("LoadPortID").Value;

            XElement mSlotsElm = carrierElm.Element("MeasurementSlots");
            if (mSlotsElm == null) {
                return new Carrier {
                    Id = carrierId,
                    LoadPurposeType = loadPurposeType,
                    LoadPortId = loadPortId,
                    SlotMap = from slotElm in carrierElm.Element("SlotMap").Elements("Slot")
                              select new SlotInfo {
                                  SlotNo = (byte)(int)slotElm.Attribute("SlotNo"),
                                  WaferID = slotElm.Value,
                              }
                };
            }

            var mSlots = (from slotElm in mSlotsElm.Elements("MeasurementSlot")
                          select (byte)(int)slotElm.Attribute("SlotNo")).ToList();

            var slotmap = (from slotElm in carrierElm.Element("SlotMap").Elements("Slot")
                           select new {
                               SlotNo = (byte)(int)slotElm.Attribute("SlotNo"),
                               WaferID = slotElm.Value,
                           }).ToList();

            var selected = new Dictionary<byte, byte>(mSlots.Count);//<實際slot,帳上slot>
            foreach (var mslot in mSlots) {
                //尋找實際量測slot
                if (slotmap.FindIndex(s => s.SlotNo == mslot) != -1
                    && !selected.ContainsKey(mslot)) {
                    selected[mslot] = mslot;
                } else {
                    for (byte up = (byte)(mslot - 1), down = (byte)(mslot + 1); up > 0 || down <= 25; up--, down++) {
                        if (slotmap.FindIndex(s => s.SlotNo == up) != -1
                           && !selected.ContainsKey(up)) {
                            selected[up] = mslot;
                            break;
                        }
                        if (slotmap.FindIndex(s => s.SlotNo == down) != -1
                            && !selected.ContainsKey(down)) {
                            selected[down] = mslot;
                            break;
                        }
                    }
                }
            }

            return new Carrier {
                Id = carrierId,
                LoadPurposeType = loadPurposeType,
                LoadPortId = loadPortId,
                SlotMap = (from s in slotmap
                           select new SlotInfo {
                               SlotNo = s.SlotNo,
                               WaferID = s.WaferID,
                               Measure = selected.ContainsKey(s.SlotNo)
                           }).ToList(),
                _MeasurementSlots = selected
            };
        }
    }
}