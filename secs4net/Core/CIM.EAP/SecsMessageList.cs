using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Secs4Net;
using System.Collections.ObjectModel;

namespace Cim.Eap {
    public sealed class SecsMessageList : ReadOnlyCollection<SecsMessage> {
        public SecsMessageList(string smlFile) : base(ParseSmlFile(smlFile).ToList()) { }

        public SecsMessage this[byte s, byte f, string name] => this[s, f].First(m => m.Name == name);

        public IEnumerable<SecsMessage> this[byte s, byte f] => this.Where(m => m.S == s && m.F == f);

        public SecsMessage this[string name] => this.First(m => m.Name == name);

        static IEnumerable<SecsMessage> ParseSmlFile(string filename) {
            using (var reader = new FileInfo(filename).OpenText()) {
                while (reader.Peek() != -1) {
                    SecsMessage secsMsg = null;
                    try {
                        secsMsg = reader.ToSecsMessage();
                    } catch (Exception ex) {
                        throw new Exception("SML parsing error before:\n" + reader.ReadToEnd(), ex);
                    }
                    if (secsMsg != null)
                        yield return secsMsg;
                }
            }
        }
    }
}
