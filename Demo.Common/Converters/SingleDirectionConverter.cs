using System;
using System.Globalization;
using System.Windows.Data;

namespace Demo.Common.Converters
{
    public abstract class SingleDirectionConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
