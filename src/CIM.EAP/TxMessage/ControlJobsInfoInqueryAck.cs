using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;

namespace Cim.Eap.Tx {
	public struct ControlJobsInfoInqueryAck : ITxMessage {
		public IEnumerable<ControlJob> ControlJobs { get; private set; }

        void ITxMessage.Parse(XElement txElm) {
            ControlJobs = from cjElm in txElm.Element("ControlJobs").Elements()
                          select new ControlJob {
                              Id = cjElm.Attribute("ControlJobID").Value,
                              StartedFlag = cjElm.Element("Started").Attribute("StartedFlag").Value == "Yes",
                              EndFlag = cjElm.Element("End").Attribute("EndFlag").Value == "Yes",
                              ProcessJobs = from processJobElm in cjElm.Element("ProcessJobs").Elements("ProcessJob")
                                            select new ProcessJob(
                                                processJobElm.Attribute("ProcessJobID").Value,
                                                processJobElm.Element("Recipe").Attribute("RecipeID").Value,
                                                string.Empty,//reticle id
                                                processJobElm.Element("DataItems").Attribute("DataCollectionDefinitionID").Value,
                                                processJobElm.Element("StartMethod").Attribute("StartMethodID").Value,
                                                from carrierElm in processJobElm.Element("Carriers").Elements("Carrier")
                                                select new Carrier {
                                                    Id = carrierElm.Attribute("CarrierID").Value,
                                                    LoadPurposeType = carrierElm.Attribute("LoadPurposeType").Value,
                                                    LoadPortId = carrierElm.Attribute("LoadPortID").Value,
                                                    SlotMap = from slotElm in carrierElm.Element("SlotMap").Elements("Slot")
                                                              select new SlotInfo {
                                                                  SlotNo = (byte)(int)slotElm.Attribute("SlotNo"),
                                                                  WaferID = slotElm.Value
                                                              }
                                                },
                                                 from paramElm in processJobElm.Element("RecipeParameters").Elements("RecipeParameter")
                                                 select new RecipeParameter {
                                                     Name = paramElm.Attribute("Name").Value,
                                                     Value = paramElm.Value
                                                 },
                                                 from lotElm in processJobElm.Element("Lots").Elements("Lot")
                                                 select new EDALotInfo {
                                                     LotID = lotElm.Attribute("LotID").Value,
                                                     ProductID = lotElm.Attribute("ProductID").Value,
                                                     OperationNo = lotElm.Attribute("OperationNo").Value,
                                                     PassCount = lotElm.Attribute("PassCount").Value,
                                                     FlowBatchID = lotElm.Attribute("FlowBatchID").Value,
                                                     Fab = lotElm.Attribute("Fab").Value,
                                                     CarrierID = lotElm.Attribute("CarrierID").Value,
                                                     RouteID = lotElm.Attribute("RouteID").Value
                                                 }
                                        )
                          };
        }
	}
}