
using System.Diagnostics;
namespace Cim.Eap.Data {
    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public struct RecipeParameter {
        public string Name;
        public string Value;
    }
}
