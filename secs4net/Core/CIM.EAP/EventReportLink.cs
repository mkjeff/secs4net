using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Secs4Net;
namespace Cim.Eap {
    public sealed class DefineLinkConfig {
        public IEnumerable<ReportID> Reports { get; }
        public IEnumerable<CEID> Events { get; }

        public DefineLinkConfig(string gemxml) {
            try {
                XDocument doc = XDocument.Load(gemxml);
                XElement gemElm = doc.Root;

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
