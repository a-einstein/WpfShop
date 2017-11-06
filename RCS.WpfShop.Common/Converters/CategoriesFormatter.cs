using System;
using System.Globalization;

namespace RCS.WpfShop.Common.Converters
{
    public class CategoriesFormatter : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversions to prevent exceptions.
            string category = values[0] as string;
            string subcategory = !string.IsNullOrEmpty(category) ? values[1] as string : null;
            string separator = !string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(subcategory) ? "/" : null;

            return $"{category} {separator} {subcategory}";
        }
    }
}