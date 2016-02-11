using System;
using System.Globalization;

namespace Demo.Common.Converters
{
    public class CategoriesFormatter : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            string category = values[0] as string;
            string subcategory = !NullOrEmpty(category) ? values[1] as string : null;
            string separator = !NullOrEmpty(category) && !NullOrEmpty(subcategory) ? "/" : null;

            return string.Format("{0} {1} {2}", category, separator, subcategory);
        }

        private static bool NullOrEmpty(string value)
        {
            return (value == null || value.Trim() == string.Empty);
        }
    }
}
