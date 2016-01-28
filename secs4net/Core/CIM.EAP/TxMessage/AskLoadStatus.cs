using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Cim.Eap.Data;
namespace Cim.Eap.Tx {
	public struct AskLoadStatusRequest : ITxMessage {
		public string LoadPortID { get; private set; }
		public string CarrierID { get; private set; }
		public string ControlJobID { get; private set; }
		public IEnumerable<ProcessJob> ProcessJobs { get; private set; }

        //<?xml version="1.0" ?>  
        //<Transaction TxName="AskLoadStatus" Type="Request" MessageKey="1211"> 
        //  <Tool ToolID="MAU01" LoadPortID="L02" CarrierID="ASR10483"/>
        //  <ControlJob ControlJobID="MAU01-20100301-0001B"/> 
        //  <ProcessJobIDs>
        //      <ProcessJob ProcessJobID="BEP527.000" >
        //          <Recipe RecipeID="A27T50R23F13" /> 
        //          <RecipeParameters></RecipeParameters>
        //      </ProcessJob>
        //  </ProcessJobIDs>
        //  <Carriers>
        //      <Carrier CarrierID="ASR10483" />
        //  </Carriers>
        //  <StartMethod StartMethodID="AUTO"/> 
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            XElement toolElm = txElm.Element("Tool");
            XElement processJobsElm = txElm.Element("ProcessJobIDs");
            LoadPortID = toolElm.Attribute("LoadPortID").Value;
            CarrierID = toolElm.Attribute("CarrierID").Value;
            ControlJobID = txElm.Element("ControlJob").Attribute("ControlJobID").Value;
            ProcessJobs = from processJobElm in txElm.Element("ProcessJobIDs").Elements("ProcessJobID")
                          select new ProcessJob(
                              processJobElm.Attribute("ProcessJobID").Value,
                              processJobElm.Element("Recipe").Attribute("RecipeID").Value,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              Enumerable.Empty<Carrier>(),
                              from recipeParameterElm in processJobElm.Element("RecipeParameters").Elements("RecipeParameters")
                              select new RecipeParameter {
                                  Name = recipeParameterElm.Attribute("Name").Value,
                                  Value = recipeParameterElm.Value
                              },
                              Enumerable.Empty<EDALotInfo>());
        }
	}
}