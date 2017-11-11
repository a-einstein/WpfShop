using RCS.WpfShop.Common.Views;
using System.Windows;
using System;
using System.Windows.Data;

namespace RCS.WpfShop.Common.Windows
{
    public partial class OkWindow : Window
    {
        public OkWindow()
        {
            InitializeComponent();
        }

        // Note this window is made specific to this MVVM concept.
        public View View
        {
            get { return viewControl.Content as View; }
            set { viewControl.Content = value; }
        }

        private bool activatedYet;

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // Prevent multiple Refresh. There does not seem to be a better event to use.
            if (!activatedYet)
            {
                activatedYet = true;

                SetBinding(Window.TitleProperty, new Binding(nameof(Title)) { Source = View?.ViewModel });

                // HACK No await as this method cannot be made async.
                View?.ViewModel?.Refresh();
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
