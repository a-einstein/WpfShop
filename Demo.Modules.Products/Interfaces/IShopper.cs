using System.Windows.Input;

namespace Demo.Modules.Products
{
    interface IShopper
    {
        ICommand CartCommand { get; set; }
    }
}
