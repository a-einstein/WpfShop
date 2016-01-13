using System;
using System.Globalization;
using System.Windows.Data;

namespace Demo.Common.Converters
{
    public class CategoriesFormatter : IMultiValueConverter 
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            string category = values[0] as string;
            string subcategory = !NullOrEmpty(category) ? values[1] as string : null;
            string separator = !NullOrEmpty(category) && !NullOrEmpty(subcategory) ? "/" : null;

            return string.Format("{0} {1} {2}", category, separator, subcategory);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }
    }
}
