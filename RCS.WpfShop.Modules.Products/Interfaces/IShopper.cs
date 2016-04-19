using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products
{
    interface IShopper
    {
        ICommand CartCommand { get; set; }
    }
}
