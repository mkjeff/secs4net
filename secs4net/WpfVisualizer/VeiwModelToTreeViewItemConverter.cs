using System;
using System.Windows.Data;
using System.Windows.Controls;

namespace SecsMessageVisuallizer
{
    class VeiwModelToTreeViewItemConverter:IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            TreeView tv = value as TreeView;
            if (tv != null) {
                TreeViewItem tvItem =tv.ItemContainerGenerator.ContainerFromItem(tv.SelectedItem) as TreeViewItem;
                if (tvItem != null)
                    return tvItem.IsFocused;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
