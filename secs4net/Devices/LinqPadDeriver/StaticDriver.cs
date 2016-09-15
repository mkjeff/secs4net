using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Extensibility.DataContext;

namespace Secs4Net.LinqPad.Deriver
{
    public class Secs4NetDriver : StaticDataContextDriver
    {
        public override string Author => "mkjeff";

        public override string Name => "Secs4Net LinqPad Driver";

        public override string GetConnectionDescription(IConnectionInfo cxInfo) 
            => cxInfo.CustomTypeInfo.GetCustomTypeDescription();

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            return new List<ExplorerItem>();
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection) 
            => new ConnectionDialog(cxInfo).ShowDialog() == true;
    }
}
