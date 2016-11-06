using Secs4Net;

namespace SecsMessageVisuallizer.ViewModel {
    public class SecsItemViewModel : TreeViewItemViewModel {
        readonly SecsItem _secsItem;
        public SecsItemViewModel(SecsItem item, SecsMessageViewModel secsMsg)
            : base(secsMsg, item.Format == SecsFormat.List && item.Items.Count > 0) {
            _secsItem = item;
        }

        public SecsItemViewModel(SecsItem item, SecsItemViewModel parentItem)
            : base(parentItem, item.Format == SecsFormat.List && item.Items.Count > 0) {
            _secsItem = item;
        }

        protected override void LoadChildren() {
            foreach (var item in _secsItem.Items) {
                base.Children.Add(new SecsItemViewModel(item, this));
            }
        }

        public string Name => _secsItem.ToString();

        public override string ToString() => _secsItem.ToString();
    }
}
