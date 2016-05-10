using System;
using System.Windows;
using System.Windows.Data;

namespace SDEMViews.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
    System.Globalization.CultureInfo culture)
        {
            var boolValue = (bool)value;

            if (boolValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
