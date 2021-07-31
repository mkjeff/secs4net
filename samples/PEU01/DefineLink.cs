using Secs4Net;

namespace Cim.Eap
{
    partial class Driver
    {
        protected override Item CeidLinkCreator(string ceid) => Item.U4(uint.Parse(ceid));

        protected override Item ReportIdLinkCreator(string reportId) => Item.U4(uint.Parse(reportId));

        protected override Item SvidLinkCreator(string svid) => Item.U4(uint.Parse(svid));

        protected override Item LinkDataIdCreator(string dataId) => Item.U1(byte.Parse(dataId));
    }
}
