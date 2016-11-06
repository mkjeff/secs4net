using Secs4Net;
using System.Threading.Tasks;
using System.Linq;
using static Secs4Net.Item;

namespace Cim.Eap {
    public abstract class EapDriver {
        public IEAP EAP { get; internal set; }
        internal protected virtual void Init() { }
        internal protected virtual void Unload() { }

        internal protected abstract SecsItem CeidLinkCreator(string ceid);
        internal protected abstract SecsItem ReportIdLinkCreator(string reportId);
        internal protected abstract SecsItem SvidLinkCreator(string svid);
        internal protected abstract SecsItem LinkDataIdCreator(string dataId);

        #region Default Define Link Message
        SecsMessage _disableEvent;
        SecsMessage _enableEvent;
        SecsMessage _disableEventReportLink;
        SecsMessage _defineEventReportLink;
        SecsMessage _disableReport;
        SecsMessage _defineReport;
        public SecsMessage S2F37_DisableEvent => _disableEvent ?? (_disableEvent =
            new SecsMessage(2, 37, "DisableEvent",
                L(
                    Boolean(false),
                    L())));

        public SecsMessage S2F37_EnableEvent => _enableEvent ?? (_enableEvent =
            new SecsMessage(2, 37, "EnableEvent",
                L(
                    Boolean(true),
                    L(
                        from ceid in EAP.EventReportLink.Events
                        select CeidLinkCreator(ceid.Id)
                    ))));

        public SecsMessage S2F35_DisableEventReportLink => _disableEventReportLink ?? (_disableEventReportLink =
            new SecsMessage(2, 35, "DisableEventReportLink",
                L(
                    LinkDataIdCreator("1"),
                    L(
                        from reportLink in EAP.EventReportLink.Events
                        where reportLink.ReportIDs.Any()
                        select L(
                            CeidLinkCreator(reportLink.Id),
                            L())))));

        public SecsMessage S2F35_DefineEventReportLink => _defineEventReportLink ?? (_defineEventReportLink =
            new SecsMessage(2, 35, "DefineEventReportLink",
                L(
                    LinkDataIdCreator("2"),
                    L(
                        from reportLink in EAP.EventReportLink.Events
                        where reportLink.ReportIDs.Any()
                        select L(
                            CeidLinkCreator(reportLink.Id),
                            L(
                                from rptid in reportLink.ReportIDs
                                select ReportIdLinkCreator(rptid)
                            ))))));

        public SecsMessage S2F33_DisableReport => _disableReport ?? (_disableReport =
            new SecsMessage(2, 33, "DisableReport",
                L(
                    LinkDataIdCreator("3"),
                    L())));

        public SecsMessage S2F33_DefineReport => _defineReport ?? (_defineReport =
            new SecsMessage(2, 33, "DefineReport",
                L(
                    LinkDataIdCreator("4"),
                    L(
                        from rptid in EAP.EventReportLink.Reports
                        where rptid.VIDs.Any()
                        select L(
                            ReportIdLinkCreator(rptid.Id),
                            L(
                                from svid in rptid.VIDs
                                select SvidLinkCreator(svid)
                            ))))));

        #endregion    

        public virtual async Task DefineLink() {
            DefineLinkSuccess(await EAP.SendAsync(S2F37_DisableEvent));
            DefineLinkSuccess(await EAP.SendAsync(S2F35_DisableEventReportLink));
            DefineLinkSuccess(await EAP.SendAsync(S2F33_DisableReport));
            DefineLinkSuccess(await EAP.SendAsync(S2F33_DefineReport));
            DefineLinkSuccess(await EAP.SendAsync(S2F35_DefineEventReportLink));
            DefineLinkSuccess(await EAP.SendAsync(S2F37_EnableEvent));
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
