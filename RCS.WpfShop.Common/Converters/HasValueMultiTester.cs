using System;
using System.Globalization;
using System.Linq;

namespace RCS.WpfShop.Common.Converters
{
    public class HasValueMultiTester : SingleDirectionMultiConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasValue = values.Aggregate(false, (current, item) => current || HasValueTester.HasValue(item));

            // Invert if any parameter is passed.
            var result = parameter != null
                ? !hasValue
                : hasValue;

            return result;
        }
    }
}
