using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Demo.Converters
{
    class WeightFormatter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Note this test should not be necessary as Weight ought to have a default of 0.
            decimal weight = values[0] != DependencyProperty.UnsetValue ? (decimal)values[0] : 0;
            string unit = values[1] as string;

            return (weight != 0)
                ? string.Format("{0} {1}", weight, unit)
                : null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
