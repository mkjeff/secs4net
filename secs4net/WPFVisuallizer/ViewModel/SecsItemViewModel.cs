using Secs4Net;

namespace SecsMessageVisuallizer.ViewModel {
    public class SecsItemViewModel : TreeViewItemViewModel {
        readonly Item _secsItem;
        public SecsItemViewModel(Item item, SecsMessageViewModel secsMsg)
            : base(secsMsg, item.Format == SecsFormat.List && item.Items.Count > 0) {
            _secsItem = item;
        }

        public SecsItemViewModel(Item item, SecsItemViewModel parentItem)
            : base(parentItem, item.Format == SecsFormat.List && item.Items.Count > 0) {
            _secsItem = item;
        }

        protected override void LoadChildren() {
            foreach (Item item in _secsItem.Items) {
                base.Children.Add(new SecsItemViewModel(item, this));
            }
        }

        public string Name {
            get {
                if(_secsItem.Format== SecsFormat.List)
                    return string.Format("{0} [{1}]",_secsItem.Format.ToSML(),_secsItem.Items.Count);
                return string.Format("{0} [{1}] {2}", _secsItem.Format.ToSML(),_secsItem.Count,_secsItem.ToString());
            }
        }

        public override string ToString() => _secsItem.ToString();
    }
}
