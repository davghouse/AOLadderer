using System;
using System.Globalization;
using System.Windows.Data;

namespace AOLadderer.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolToNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
