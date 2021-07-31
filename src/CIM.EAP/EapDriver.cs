using Secs4Net;
using System.Threading.Tasks;
using System.Linq;

namespace Cim.Eap {
    public abstract class EapDriver {
        public IEAP EAP { get; internal set; }
        internal protected virtual void Init() { }
        internal protected virtual void Unload() { }

        internal protected abstract Item CeidLinkCreator(string ceid);
        internal protected abstract Item ReportIdLinkCreator(string reportId);
        internal protected abstract Item SvidLinkCreator(string svid);
        internal protected abstract Item LinkDataIdCreator(string dataId);

        #region Default Define Link Message
        SecsMessage _disableEvent;
        SecsMessage _enableEvent;
        SecsMessage _disableEventReportLink;
        SecsMessage _defineEventReportLink;
        SecsMessage _disableReport;
        SecsMessage _defineReport;
        public SecsMessage S2F37_DisableEvent => _disableEvent ?? (_disableEvent =
            new SecsMessage(2, 37, "DisableEvent",
                Item.L(
                    Item.Boolean(false),
                    Item.L())));

        public SecsMessage S2F37_EnableEvent => _enableEvent ?? (_enableEvent =
            new SecsMessage(2, 37, "EnableEvent",
                Item.L(
                    Item.Boolean(true),
                    Item.L(
                        from ceid in EAP.EventReportLink.Events
                        select CeidLinkCreator(ceid.Id)
                    ))));

        public SecsMessage S2F35_DisableEventReportLink => _disableEventReportLink ?? (_disableEventReportLink =
            new SecsMessage(2, 35, "DisableEventReportLink",
                Item.L(
                    LinkDataIdCreator("1"),
                    Item.L(
                        from reportLink in EAP.EventReportLink.Events
                        where reportLink.ReportIDs.Any()
                        select Item.L(
                            CeidLinkCreator(reportLink.Id),
                            Item.L())))));

        public SecsMessage S2F35_DefineEventReportLink => _defineEventReportLink ?? (_defineEventReportLink =
            new SecsMessage(2, 35, "DefineEventReportLink",
                Item.L(
                    LinkDataIdCreator("2"),
                    Item.L(
                        from reportLink in EAP.EventReportLink.Events
                        where reportLink.ReportIDs.Any()
                        select Item.L(
                            CeidLinkCreator(reportLink.Id),
                            Item.L(
                                from rptid in reportLink.ReportIDs
                                select ReportIdLinkCreator(rptid)
                            ))))));

        public SecsMessage S2F33_DisableReport => _disableReport ?? (_disableReport =
            new SecsMessage(2, 33, "DisableReport",
                Item.L(
                    LinkDataIdCreator("3"),
                    Item.L())));

        public SecsMessage S2F33_DefineReport => _defineReport ?? (_defineReport =
            new SecsMessage(2, 33, "DefineReport",
                Item.L(
                    LinkDataIdCreator("4"),
                    Item.L(
                        from rptid in EAP.EventReportLink.Reports
                        where rptid.VIDs.Any()
                        select Item.L(
                            ReportIdLinkCreator(rptid.Id),
                            Item.L(
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
