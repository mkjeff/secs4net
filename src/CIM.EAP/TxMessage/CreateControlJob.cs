using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;
namespace Cim.Eap.Tx {
	public struct CreateControlJobRequest : ITxMessage {
		public string ControlJobID { get; private set; }
		public bool HOQ { get; private set; }
		public IEnumerable<string> ProcessJobIDs { get; private set; }
		public IEnumerable<string> CarrierIDs { get; private set; }

        //<?xml version="1.0" ?>  
        //<Transaction TxName="CreateControlJob" Type="Request" MessageKey="1216">
        //  <Tool ToolID="MAU01" /> 
        //  <ControlJob ControlJobID="MAU01-20100301-0001B" CJHOQ="FALSE"/> 
        //  <ProcessJobIDs> 
        //      <ProcessJob ProcessJobID="BEP527.000" /> 
        //  </ProcessJobIDs> 
        //  <Carriers> 
        //      <Carrier CarrierID="ASR10483" LoadPurposeType="Process Lot" LoadPortID="L02">
        //      </Carrier>
        //  </Carriers> 
        //  <StartMethod StartMethodID="AUTO"/> 
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            ControlJobID = txElm.Element("ControlJob").Attribute("ControlJobID").Value;
            HOQ = Convert.ToBoolean(((string)txElm.Element("ControlJob").Attribute("CJHOQ")));
            ProcessJobIDs = from pjElm in txElm.Element("ProcessJobIDs").Elements("ProcessJob")
                            select pjElm.Attribute("ProcessJobID").Value;
            CarrierIDs = from carrierElm in txElm.Element("Carriers").Elements("Carrier")
                         select carrierElm.Attribute("CarrierID").Value;
        }
	}
}