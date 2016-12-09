using System.Linq;
using Cim.Eap.Data;
using Cim.Eap.Tx;
using System.Threading.Tasks;
using Cim.Eap;

namespace Eap.Driver.MWN {
    partial class Driver {

        async Task TCS_AskLoadtStatus(AskLoadStatusRequest tx) {
            await CheckPJDuplicated(tx);
            await CheckPJSpace();
            await CheckCJSpace();
            await CheckRecipe(tx);
        }

        async Task CheckPJDuplicated(AskLoadStatusRequest tx) {
            var s16f20 = await EAP.SendAsync(EAP.SecsMessages[16, 19, "CheckPJDuplicated"]);
            foreach (ProcessJob pj in tx.ProcessJobs)
                foreach (var item in s16f20.SecsItem.Items)
                    if ((string)item.Items[0] == pj.Id)
                        throw new ScenarioException("CheckPJDuplicated Error: ProcessJobID(\'" + pj.Id + "\') is exist!");
        }

        async Task CheckPJSpace() {
            var s16f22 = await EAP.SendAsync(EAP.SecsMessages[16, 21, "CheckPJSpace"]);
            if ((ushort)s16f22.SecsItem == 0)
                throw new ScenarioException("ProcessJob Space is 0");
        }

        async Task CheckCJSpace() {
            var s1f4 = await EAP.SendAsync(EAP.SecsMessages[1, 3, "CheckCJSpace"]);
            if ((byte)s1f4.SecsItem.Items[0] == 0)
                throw new ScenarioException("ControlJob Space is 0");
        }

        async Task CheckRecipe(AskLoadStatusRequest tx) {
            var s7f20 = await EAP.SendAsync(EAP.SecsMessages[7, 19, "GetRecipeList"]);
            var recipeList = from item in s7f20.SecsItem.Items
                             select (string)item;

            foreach (var pj in tx.ProcessJobs)
                if (!recipeList.Any(ppid => ppid == pj.RecipeId))
                    throw new ScenarioException("PPID(\'" + pj.RecipeId + "\') Not Found.");
        }
    }
}