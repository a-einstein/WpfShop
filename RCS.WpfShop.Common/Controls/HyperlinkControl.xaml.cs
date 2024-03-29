﻿using System;
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
            get => (Uri)GetValue(NavigateUriProperty);
            set => SetValue(NavigateUriProperty, value);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute=true
            };

            Process.Start(processStartInfo);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(HyperlinkControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
