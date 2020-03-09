using System;
using System.Diagnostics;
using System.Windows;

namespace RCS.WpfShop.Common.Navigation
{
    [DebuggerDisplay("DisplayName = {DisplayName}, Uri = {Uri.OriginalString}")]
    public class Destination : DependencyObject
    {
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register(nameof(DisplayName), typeof(string), typeof(Destination));

        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register(nameof(Uri), typeof(Uri), typeof(Destination));

        public Uri Uri
        {
            get => (Uri)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }
    }
}
