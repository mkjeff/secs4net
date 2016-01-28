using System.Linq;
using Cim.Eap.Data;
using Cim.Eap.Tx;
namespace Cim.Eap {
    partial class Driver {

        void TCS_AskLoadtStatus(AskLoadStatusRequest tx) {
            CheckPJDuplicated(tx);
            CheckPJSpace();
            CheckCJSpace();
            CheckRecipe(tx);
        }

        void CheckPJDuplicated(AskLoadStatusRequest tx) {
            var s16f20 = EAP.Send(EAP.SecsMessages[16, 19, "CheckPJDuplicated"]);
            foreach (ProcessJob pj in tx.ProcessJobs)
                foreach (var item in s16f20.SecsItem.Items)
                    if ((string)item.Items[0] == pj.Id)
                        throw new ScenarioException("CheckPJDuplicated Error: ProcessJobID(\'" + pj.Id + "\') is exist!");
        }

        void CheckPJSpace() {
            var s16f22 = EAP.Send(EAP.SecsMessages[16, 21, "CheckPJSpace"]);
            if ((ushort)s16f22.SecsItem == 0)
                throw new ScenarioException("ProcessJob Space is 0");
        }

        void CheckCJSpace() {
            var s1f4 = EAP.Send(EAP.SecsMessages[1, 3, "CheckCJSpace"]);
            if ((byte)s1f4.SecsItem.Items[0] == 0)
                throw new ScenarioException("ControlJob Space is 0");
        }

        void CheckRecipe(AskLoadStatusRequest tx) {
            var s7f20 = EAP.Send(EAP.SecsMessages[7, 19, "GetRecipeList"]);
            var recipeList = from item in s7f20.SecsItem.Items
                             select (string)item;

            foreach (var pj in tx.ProcessJobs)
                if (!recipeList.Any(ppid => ppid == pj.RecipeId))
                    throw new ScenarioException("PPID(\'" + pj.RecipeId + "\') Not Found.");
        }
    }
}