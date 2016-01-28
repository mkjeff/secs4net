using System;
using System.Windows.Data;

namespace SecsMessageVisuallizer
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class ReplyExpectedToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => ((bool)value) ? "'W'" : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
