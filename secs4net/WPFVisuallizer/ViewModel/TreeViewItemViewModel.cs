using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SecsMessageVisuallizer.ViewModel
{
    public class TreeViewItemViewModel : INotifyPropertyChanged {
        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        readonly ObservableCollection<TreeViewItemViewModel> _children;
        readonly TreeViewItemViewModel _parent;

        bool _isExpanded;
        bool _isSelected;

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren) {
            _parent = parent;

            _children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
                _children.Add(DummyChild);
        }

        // This is used to create the DummyChild instance.
        private TreeViewItemViewModel() {
        }

        /// <summary>
        /// Returns the logical child items of this object.
        /// </summary>
        public ObservableCollection<TreeViewItemViewModel> Children => _children;

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild => Children.Count == 1 && Children[0] == DummyChild;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded {
            get { return _isExpanded; }
            set {
                if (value != _isExpanded) {
                    _isExpanded = value;
                    OnPropertyChanged();
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (HasDummyChild) {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
            }
        }

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected {
            get { return _isSelected; }
            set { SetField(ref _isSelected, value); }
        }

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren() {
        }

        public TreeViewItemViewModel Parent => _parent;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
