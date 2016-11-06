using Secs4Net;

namespace Cim.Eap
{
    partial class Driver
    {
        protected override SecsItem CeidLinkCreator(string ceid) => Item.U4(uint.Parse(ceid));

        protected override SecsItem ReportIdLinkCreator(string reportId) => Item.U4(uint.Parse(reportId));

        protected override SecsItem SvidLinkCreator(string svid) => Item.U4(uint.Parse(svid));

        protected override SecsItem LinkDataIdCreator(string dataId) => Item.U1(byte.Parse(dataId));
    }
}
