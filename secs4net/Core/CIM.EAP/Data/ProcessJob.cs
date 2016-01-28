using System.Collections.Generic;
using System.Xml.Linq;
using System.Diagnostics;
using System.Linq;

namespace Cim.Eap.Data {
    [DebuggerDisplay("{Id,nq}")]
    public sealed class ProcessJob {
        public readonly string Id;
        public readonly string RecipeId;
        public readonly string ReticleId;
        public readonly string DataCollectionDefinitionID;
        public readonly string StartMethod;
        public readonly IEnumerable<RecipeParameter> RecipeParameters;
        public readonly IEnumerable<Carrier> Carriers;
        public readonly IEnumerable<EDALotInfo> EDALotInfos;

        public static readonly ProcessJob DummyProcessJob;
        static ProcessJob() {
            string na = "N/A";
            DummyProcessJob = new ProcessJob(
                "OfflineJob", na, na, na, na,
                new[]{
                    new Carrier {
                        Id = na,
                        LoadPortId = na,
                        LoadPurposeType = na,
                        SlotMap = from i in Enumerable.Range(1, 25)
                                  select new SlotInfo {
                                    SlotNo = (byte)i,
                                    WaferID = "Wafer_" + i
                                  }
                    }
                },
                Enumerable.Empty<RecipeParameter>(),
                Enumerable.Empty<EDALotInfo>());
        }

        internal ProcessJob(
                string id, 
                string recipeId, 
                string reticleId, 
                string dcId, 
                string startMethod,
                IEnumerable<Carrier> carriers, 
                IEnumerable<RecipeParameter> recipeParameters, 
                IEnumerable<EDALotInfo> edaLots) {
            Id = id;
            RecipeId = recipeId;
            ReticleId = reticleId;
            DataCollectionDefinitionID = dcId;
            StartMethod = startMethod;
            Carriers = carriers.ToList();
            RecipeParameters = recipeParameters.ToList();
            EDALotInfos = edaLots.ToList();
        }
    }
}
