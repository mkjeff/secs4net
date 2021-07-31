using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LINQPad.Extensibility.DataContext;
using Newtonsoft.Json;
using Secs4Net.Json;
using Secs4Net.Rx;

namespace Secs4Net.LinqPad.Deriver
{
    public class SecsMessageObservableDriver : StaticDataContextDriver
    {
        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            yield return typeof(Item).Assembly.Location;
            yield return typeof(JsonExtension).Assembly.Location;
            yield return typeof(SecsObservableDataContext).Assembly.Location;
            yield return typeof(JsonConvert).Assembly.Location;
            yield return typeof(Observer).Assembly.Location;
            yield return typeof(ISubject<>).Assembly.Location;
            yield return typeof(Subject).Assembly.Location;
            yield return typeof(EnumerableEx).Assembly.Location;
            yield return typeof(Unsafe).Assembly.Location;
        }

        public override IEnumerable<string> GetNamespacesToAdd(IConnectionInfo cxInfo)
        {
            yield return "Secs4Net";
            yield return "System.Reactive.Linq";
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2) 
            => c1.DriverData.ToString() == c2.DriverData.ToString();

        public override void InitializeContext(IConnectionInfo cxInfo, object context, QueryExecutionManager executionManager)
        {
            base.InitializeContext(cxInfo, context, executionManager);
        }

        public override string Author => "mkjeff";

        public override string Name => "Secs4Net LinqPad Driver";

        public override string GetConnectionDescription(IConnectionInfo cxInfo)
            => cxInfo.CustomTypeInfo.GetCustomTypeDescription();

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            try
            {
                var msgs = File.OpenText(cxInfo.DriverData.Attribute("file").Value).ToSecsMessages();
                var query =
                    from a in msgs
                    where a.F % 2 == 1 && !string.IsNullOrWhiteSpace(a.Name)
                    let ei = a.SecsItem.ToExplorerItem()
                    select ei != null
                    ? new ExplorerItem($"S{a.S}F{a.F} {a.Name}", ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                    {
                        Children = new List<ExplorerItem>(1) { ei }
                    }
                    : new ExplorerItem($"S{a.S}F{a.F} {a.Name}", ExplorerItemKind.QueryableObject, ExplorerIcon.Table);

                return query.ToList();
            }
            catch
            {
                return new List<ExplorerItem>();
            }
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
            => new ConnectionDialog(cxInfo).ShowDialog() == true;
    }

    static class ExtensionMethod
    {
        public static ExplorerItem ToExplorerItem(this Item item)
        {
            if (item == null || item.Count == 0)
                return null;

            switch (item.Format)
            {
                case SecsFormat.List:
                    return new ExplorerItem(item, ExplorerItemKind.Property, ExplorerIcon.OneToMany)
                    {
                        Children = (from a in item.Items
                                    let ei = a.ToExplorerItem()
                                    where ei != null
                                    select ei).ToList()
                    };
                default:
                    return new ExplorerItem(item, ExplorerItemKind.Property, ExplorerIcon.Column);
            }
        }
    }
}
