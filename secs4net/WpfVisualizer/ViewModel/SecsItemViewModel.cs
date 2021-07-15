using Secs4Net;
using Secs4Net.Json;
using System.Text.Json;

namespace SecsMessageVisuallizer.ViewModel
{
    public class SecsItemViewModel : TreeViewItemViewModel
    {
        readonly Item _secsItem;
        public SecsItemViewModel(Item item, SecsMessageViewModel secsMsg)
            : base(secsMsg, item.Format == SecsFormat.List && item.Items.Count > 0)
        {
            _secsItem = item;
        }

        public SecsItemViewModel(Item item, SecsItemViewModel parentItem)
            : base(parentItem, item.Format == SecsFormat.List && item.Items.Count > 0)
        {
            _secsItem = item;
        }

        protected override void LoadChildren()
        {
            foreach (Item item in _secsItem.Items)
            {
                base.Children.Add(new SecsItemViewModel(item, this));
            }
        }

        public string Name 
            => _secsItem.ToString();

        public override string ToString() 
            => _secsItem.ToJson(new JsonWriterOptions{ Indented = true});
    }
}
