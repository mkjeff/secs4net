using Secs4Net;
using Secs4Net.Json;
using System.Text.Json;

namespace SecsMessageVisuallizer.ViewModel
{
    public class SecsItemViewModel : TreeViewItemViewModel
    {
        readonly Item _secsItem;
        public SecsItemViewModel(Item item, SecsMessageViewModel secsMsg)
            : base(secsMsg, lazyLoadChildren: item.Format == SecsFormat.List && item.Count > 0)
        {
            _secsItem = item;
        }

        public SecsItemViewModel(Item item, SecsItemViewModel parentItem)
            : base(parentItem, lazyLoadChildren: item.Format == SecsFormat.List && item.Count > 0)
        {
            _secsItem = item;
        }

        protected override void LoadChildren()
        {
            foreach (Item item in _secsItem)
            {
                base.Children.Add(new SecsItemViewModel(item, this));
            }
        }

        public string Name
            => _secsItem.ToString();

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new ItemJsonConverter(),
            }
        };

        public override string ToString()
            => JsonSerializer.Serialize(_secsItem, JsonOptions);
    }
}
