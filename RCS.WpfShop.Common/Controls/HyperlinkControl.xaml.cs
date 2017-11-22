using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RCS.WpfShop.Common.Controls
{
    /// <summary>
    /// Hyperlink (flow content) turned into control, tailored to Text link.
    /// </summary>
    public partial class HyperlinkControl : UserControl
    {
        public HyperlinkControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NavigateUriProperty =
            DependencyProperty.Register(nameof(NavigateUri), typeof(Uri), typeof(HyperlinkControl));

        public Uri NavigateUri
        {
            get { return (Uri)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(HyperlinkControl));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
