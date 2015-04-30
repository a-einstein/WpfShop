using WpfTestApplication.BaseClasses;
using ShoppingCartDataTable = WpfTestApplication.Data.ProductsDataSet.ShoppingCartDataTable;

namespace WpfTestApplication.ViewModels
{
    class ShoppingCartViewModel : ItemsViewModel
    {
        protected override void LoadData()
        {
            ShoppingCartDataTable shoppingCartDataTable = new ShoppingCartDataTable();
            Items = shoppingCartDataTable.DefaultView;
        }
    }
}
