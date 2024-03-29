﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace RCS.WpfShop.Common.Converters
{
    public class ByteArrayToBitmapImageConverter : SingleDirectionConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value);
        }

        private static BitmapImage Convert(object value)
        {
            if (value is byte[] byteArray)
            {
                var memoryStream = new MemoryStream(byteArray);

                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream; // Bijeffect: omzetting JPEG -> ARGB.
                bitmapImage.EndInit();

                return bitmapImage;
            }

            return null;
        }
    }
}
