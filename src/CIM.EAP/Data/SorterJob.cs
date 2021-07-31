using System.Collections.Generic;

namespace Cim.Eap.Data {
    public struct SorterJob {
        public string ControlJobId;
        public string ActionCode;
        internal bool OnRoute;
        internal int SourceCarrierCount;
        public IEnumerable<RecipeParameter> RecipeParameters;
        public Carrier SourceMap;
        public Carrier DestinationMap;
    }
}
