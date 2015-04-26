using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data.ProductsDataSetTableAdapters;
using ProductDetailsDataTable = WpfTestApplication.Data.ProductsDataSet.ProductDetailsDataTable;
using ProductDetailsRow = WpfTestApplication.Data.ProductsDataSet.ProductDetailsRow;

namespace WpfTestApplication.ViewModels
{
    class ProductViewModel : ItemViewModel<ProductDetailsRow>
    {
        protected override void LoadData()
        {
            ProductDetailsTableAdapter productTableAdapter = new ProductDetailsTableAdapter();

            ProductDetailsDataTable productDetailsTable = productTableAdapter.GetDataBy(ItemId);

            Item = productDetailsTable.FindByProductID(ItemId);
        }
    }
}