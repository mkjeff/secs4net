using System.Xml.Linq;

namespace Online.Eap.Tx {
    public struct PPBodyQueryRequest : ITxMessage {
        public string RecipeId { get; private set; }
        public string Type { get; private set; }
        public string PPID { get; private set; }

        XElement datasElm;

        //<?xml version="1.0" encoding="utf-16"?>
        //<Transaction TxName="PPBodyQuery" Type="Request" MessageKey="8376">
        //  <Tool ToolID="MAU01" />
        //  <Recipe>
        //    <RecipeID>A27T50R23F13</RecipeID>
        //    <TYPE>Sequence</TYPE>
        //    <PPID>A27T50R23F13</PPID>
        //  </Recipe>
        //</Transaction>
        void ITxMessage.Parse(XElement txElm) {
            XElement recieElm = txElm.Element("Recipe");
            this.RecipeId = recieElm.Element("RecipeID").Value;
            this.Type = recieElm.Element("TYPE").Value;
            this.PPID = recieElm.Element("PPID").Value;
            recieElm.Add(new XElement("PPBody", datasElm = new XElement("DATAs")));
        }

        public void Add(PPBodyItem item) {
            datasElm.Add(new XElement("DATA",
                new XAttribute("ID1",item.ID1??"*"),
                new XAttribute("ID2",item.ID2??"*"),
                new XAttribute("ID3",item.ID3??"*"),
                new XAttribute("ID4",item.ID4??"*"),
                new XAttribute("Value",item.Value)));
        }
    }

    public struct PPBodyItem {
        public string ID1, ID2, ID3, ID4, Value;
    }
}
