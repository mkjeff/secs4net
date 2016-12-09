using System.Collections.Generic;
using System.IO;
using System.Linq;
using Secs4Net;
using System.Collections.ObjectModel;
using Secs4Net.Sml;

namespace Cim.Eap
{
    public sealed class SecsMessageList : ReadOnlyCollection<SecsMessage> {
        public SecsMessageList(string jsonFile) : base(File.OpenText(jsonFile).ToSecsMessages().ToList()) { }

        public SecsMessage this[byte s, byte f, string name] => this[s, f].First(m => m.Name == name);

        public IEnumerable<SecsMessage> this[byte s, byte f] => this.Where(m => m.S == s && m.F == f);

        public SecsMessage this[string name] => this.First(m => m.Name == name);
    }
}
