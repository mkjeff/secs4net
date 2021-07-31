using Online.Eap.Tx;
using Secs4Net;
namespace Online.Eap {
    partial class Driver {
        void HandleTCS(PPBodyQueryRequest tx) {
            if (tx.Type != "Body")
                return;

            var s7f6 = EAP.Send(new SecsMessage(7, 5, "PPBodyQuery", Item.A(tx.PPID)));

            if (s7f6.SecsItem.Items[0].ToString() != tx.PPID)
                throw new ScenarioException("PPID unmatch!!");

            if (s7f6.SecsItem.Items.Count == 0)
                throw new ScenarioException("PPBody is null");

            string body = s7f6.SecsItem.Items[1].ToString();

            if (EAP.ToolId.StartsWith("PEU")) {
                tx.Add( new PPBodyItem {
                    ID1 = "IrrMethod_Ref",
                    Value = body.Substring(0x48, 3).Trim()
                });

                tx.Add(new PPBodyItem {
                    ID1 = "Exposure_Time",
                    Value = body.Substring(0x4d, 3).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "Exposure_Intensity",
                    Value = body.Substring(0x51, 1).Trim()
                });
            } else if (EAP.ToolId.StartsWith("PGU")) {
                tx.Add( new PPBodyItem {
                    ID1 = "RecipeName",
                    Value = body.Substring(0, 48).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "RecipeEditDate",
                    Value = body.Substring(49, 10).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "RecipeEditTime",
                    Value = body.Substring(59, 9).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "RecipeValidity",
                    Value = body.Substring(68, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationTimeIntensity_u1",
                    Value = body.Substring(70, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationTimeIntensity_u2",
                    Value = body.Substring(75, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationTimeIntensity_u3",
                    Value = body.Substring(80, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationTimeIntensity_u4",
                    Value = body.Substring(85, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationTimeIntensity_u5",
                    Value = body.Substring(90, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationIntensity_i1",
                    Value = body.Substring(95, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationIntensity_i2",
                    Value = body.Substring(97, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationIntensity_i3",
                    Value = body.Substring(99, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationIntensity_i4",
                    Value = body.Substring(101, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "IrradiationIntensity_i5",
                    Value = body.Substring(103, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTimeInterval_t1",
                    Value = body.Substring(105, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTimeInterval_t2",
                    Value = body.Substring(110, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTimeInterval_t3",
                    Value = body.Substring(115, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTimeInterval_t4",
                    Value = body.Substring(120, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTimeInterval_t5",
                    Value = body.Substring(125, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTemperature_T1",
                    Value = body.Substring(130, 4).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTemperature_T2",
                    Value = body.Substring(134, 4).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTemperature_T3",
                    Value = body.Substring(138, 4).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTemperature_T4",
                    Value = body.Substring(142, 4).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "HotPlateTemperature_T5",
                    Value = body.Substring(146, 4).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "CoolingStageUsage",
                    Value = body.Substring(150, 2).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "CoolingTimeAtTheCoolingStage",
                    Value = body.Substring(152, 5).Trim()
                });

                tx.Add( new PPBodyItem {
                    ID1 = "PurgeDuringProcess",
                    Value = body.Substring(157, 2).Trim()
                });
            }
        }
    }
}