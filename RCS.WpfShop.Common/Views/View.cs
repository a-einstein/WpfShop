using RCS.WpfShop.Common.ViewModels;
using System.Windows.Controls;

namespace RCS.WpfShop.Common.Views
{
    public abstract class View : UserControl
    {
        public ViewModel ViewModel
        {
            get { return DataContext as ViewModel; }
            set { DataContext = value; }
        }
    }
}
