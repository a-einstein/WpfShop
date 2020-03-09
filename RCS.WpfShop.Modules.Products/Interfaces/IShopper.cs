using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products
{
    internal interface IShopper
    {
        // Note that set cannot be made private here.
        ICommand CartCommand { get; set; }
    }
}
