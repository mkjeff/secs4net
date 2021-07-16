using System;
using System.Windows.Data;
using System.Windows.Controls;

namespace SecsMessageVisuallizer
{
    class VeiwModelToTreeViewItemConverter:IValueConverter {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value is TreeView tv)
            {
                if (tv.ItemContainerGenerator.ContainerFromItem(tv.SelectedItem) is TreeViewItem tvItem)
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
