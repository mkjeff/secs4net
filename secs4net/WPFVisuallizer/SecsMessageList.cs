using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using Secs4Net.Json;

namespace Secs4Net
{
    public sealed class SecsMessageList : ObservableCollection<SecsMessage> {
        public SecsMessageList(string jsonFile) : base(File.OpenText(jsonFile).ToSecsMessages()) { }

        public SecsMessage this[byte s, byte f, string name] => this[s, f].FirstOrDefault(m => m.Name == name);

        public IEnumerable<SecsMessage> this[byte s, byte f] => this.Where(m => m.S == s && m.F == f);

        public SecsMessage this[string name] => this.FirstOrDefault(m => m.Name == name);
    }
}
