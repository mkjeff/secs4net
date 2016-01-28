using System.Collections.Generic;
using Online.Eap.Tx;
using Secs4Net;
namespace Online.Eap {
    partial class Driver {
        readonly IDictionary<ushort, string> _nameMap = new Dictionary<ushort, string> {
            {1  ,"Process Time" },
            {2  ,"Ar Flow"      },
            {3  ,"N2 Flow"      },
            {10 ,"DC Power"     },
        };

        void HandleTCS(PPBodyQueryRequest tx) {
            if (tx.Type != "Sequence")
                return;

            var s7f26 = EAP.Send(new SecsMessage(7, 25, "PPBodyQuery", Item.A(tx.PPID)));

            if (s7f26.SecsItem.Items[0].ToString() != tx.PPID)
                throw new ScenarioException("PPID unmatch!!!");

            if (s7f26.SecsItem.Items[3].Count < 5)
                throw new ScenarioException("PPBody is empty");

            var bodyList = s7f26.SecsItem.Items[3].Items;
            for (int i = 0; i < bodyList.Count; i++) {
                Item item = bodyList[i];
                string chamberItem = item.Items[1].Items[0].ToString();

                switch (chamberItem) {
                    case "LL-H": //Step1 LL-H RecipeName
                        tx.Add(new PPBodyItem {
                            ID1 = "1",
                            ID2 = chamberItem,
                            Value = item.Items[1].Items[1].ToString()
                        });
                        break;
                    case "B": //Step2 B RecipeName 
                    case "C": //Step2 C RecipeName 
                    case "D": //Step2 D RecipeName 
                        string chamberRecipe = item.Items[1].Items[1].ToString();
                        if (chamberRecipe.Contains("Shutter"))
                            continue;

                        tx.Add(new PPBodyItem {
                            ID1 = "2",
                            ID2 = chamberItem,
                            Value = chamberRecipe
                        });

                        int stepCount = int.Parse(item.Items[1].Items[2].ToString());
                        int paramNum = int.Parse(item.Items[1].Items[3].ToString());
                        for (int j = i + 1; j < i + paramNum + 1; j++) {
                            item = bodyList[j];
                            string itemName;
                            if (_nameMap.TryGetValue(item.Items[0].FirstValue<ushort>(), out itemName)) {
                                for (int step = 0; step < stepCount; step++) {
                                    tx.Add(new PPBodyItem {
                                        ID1 = "2",
                                        ID2 = chamberItem,
                                        ID3 = "Step" + (step + 1),
                                        ID4 = itemName,
                                        Value = item.Items[1].Items[1 + 4 * step].ToString()
                                    });
                                }
                            }
                        }
                        i += paramNum;
                        break;
                }
            }
        }
    }
}