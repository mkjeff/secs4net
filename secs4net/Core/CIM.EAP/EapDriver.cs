using Secs4Net;

namespace Cim.Eap {
    public class EapDriver {
        public IEAP EAP { get; internal set; }
        internal protected virtual void Init() { }
        internal protected virtual void Unload() { }

        public virtual void DefineLink() {
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F37_DisableEvent));
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F35_DisableEventReportLink));
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F33_DisableReport));
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F33_DefineReport));
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F35_DefineEventReportLink));
            DefineLinkSuccess(EAP.Send(EAP.EventReportLink.S2F37_EnableEvent));
        }

        static void DefineLinkSuccess(SecsMessage msg) {
            if (msg.SecsItem.GetValue<byte>() != 0)
                throw new ScenarioException("Define Link failed : " + msg.Name);
        }
    }

    public interface IConfigurable {
        void LoadConfig();
    }
}
