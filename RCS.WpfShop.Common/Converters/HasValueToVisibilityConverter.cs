using System;
using System.Globalization;
using System.Windows;

namespace RCS.WpfShop.Common.Converters
{
    public class HasValueToVisibilityConverter : HasValueTester
    {
        public override object Convert(object testObject, Type targetType, object parameter, CultureInfo culture)
        {
            var testResult = (bool)base.Convert(testObject, targetType, parameter, culture);

            return testResult ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}