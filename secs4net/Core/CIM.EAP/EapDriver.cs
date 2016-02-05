using Secs4Net;
using System.Threading.Tasks;

namespace Cim.Eap {
    public abstract class EapDriver {
        public IEAP EAP { get; internal set; }
        internal protected virtual void Init() { }
        internal protected virtual void Unload() { }

        public virtual async Task DefineLink() {
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F37_DisableEvent));
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F35_DisableEventReportLink));
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F33_DisableReport));
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F33_DefineReport));
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F35_DefineEventReportLink));
            DefineLinkSuccess(await EAP.SendAsync(EAP.EventReportLink.S2F37_EnableEvent));
        }

        static void DefineLinkSuccess(SecsMessage msg) {
            if (msg.SecsItem.GetValue<byte>() != 0)
                throw new ScenarioException("Define Link failed : " + msg.Name);
        }

        internal protected abstract void HandleToolAlarm(SecsMessage msg);
    }

    public interface IConfigurable {
        void LoadConfig();
    }
}
