using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Secs4Net;
namespace Cim.Eap {
    public sealed class DefineLinkConfig {
        public SecsFormat CeidFormat { get; }
        public SecsFormat SvidFormat { get; }
        public SecsFormat ReportIdFormat { get; }
        public SecsFormat DataIdFormat { get; }
        public IEnumerable<ReportID> Reports { get; }
        public IEnumerable<CEID> Events { get; }

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
                from ceid in Events
                select CeidFormat.Create(ceid.Id)
            ))));

        public SecsMessage S2F35_DisableEventReportLink => _disableEventReportLink ?? (_disableEventReportLink =
    new SecsMessage(2, 35, "DisableEventReportLink",
        Item.L(
            DataIdFormat.Create("1"),
            Item.L(
                from reportLink in Events
                where reportLink.ReportIDs.Any()
                select Item.L(
                    CeidFormat.Create(reportLink.Id),
                    Item.L())))));

        public SecsMessage S2F35_DefineEventReportLink => _defineEventReportLink ?? (_defineEventReportLink =
    new SecsMessage(2, 35, "DefineEventReportLink",
        Item.L(
            DataIdFormat.Create("2"),
            Item.L(
                from reportLink in Events
                where reportLink.ReportIDs.Any()
                select Item.L(
                    CeidFormat.Create(reportLink.Id),
                    Item.L(
                        from rptid in reportLink.ReportIDs
                        select ReportIdFormat.Create(rptid)
                    ))))));

        public SecsMessage S2F33_DisableReport => _disableReport ?? (_disableReport =
    new SecsMessage(2, 33, "DisableReport",
        Item.L(
            DataIdFormat.Create("3"),
            Item.L())));

        public SecsMessage S2F33_DefineReport => _defineReport ?? (_defineReport =
    new SecsMessage(2, 33, "DefineReport",
        Item.L(
            DataIdFormat.Create("4"),
            Item.L(
                from rptid in Reports
                where rptid.VIDs.Any()
                select Item.L(
                    ReportIdFormat.Create(rptid.Id),
                    Item.L(
                        from svid in rptid.VIDs
                        select SvidFormat.Create(svid)
                    ))))));

        #endregion    

        public DefineLinkConfig(string gemxml) {
            try {
                XDocument doc = XDocument.Load(gemxml);
                XElement gemElm = doc.Root;

                XElement formatElm = gemElm.Element("DataFormats");
                CeidFormat = formatElm.Element("CEID").Attribute("Type").Value.ToEnum<SecsFormat>();
                SvidFormat = formatElm.Element("VID").Attribute("Type").Value.ToEnum<SecsFormat>();
                ReportIdFormat = formatElm.Element("ReportID").Attribute("Type").Value.ToEnum<SecsFormat>();
                DataIdFormat = formatElm.Element("DataID").Attribute("Type").Value.ToEnum<SecsFormat>();

                Reports = (from elm in gemElm.Element("Reports").Elements("Report")
                           select new ReportID {
                               Id = elm.Attribute("ID").Value,
                               VIDs = (from vidElm in elm.Elements("VID")
                                       select vidElm.Attribute("ID").Value).ToList()
                           }).ToList();

                Events = (from elm in gemElm.Element("Events").Elements("Event")
                          select new CEID {
                              Id = elm.Attribute("ID").Value,
                              Name = elm.Attribute("Name").Value,
                              ReportIDs = (from reportElm in elm.Elements("Report")
                                           select reportElm.Attribute("ID").Value).ToList()
                          }).ToList();
            } catch (Exception ex) {
                throw new ApplicationException("DefineLink config loading error: " + ex.Message);
            }
        }
    }

    public struct ReportID {
        public string Id;
        public IEnumerable<string> VIDs;
    }

    public struct CEID {
        public string Id;
        public string Name;
        public IEnumerable<string> ReportIDs;
    }
}
