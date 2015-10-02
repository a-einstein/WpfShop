using System;
using System.Globalization;
using System.Windows.Data;

namespace Demo.Converters
{
    public class SizeFormatter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            return string.Format("{0} {1}", values[0] as string, values[1] as string);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
