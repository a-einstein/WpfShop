using WpfTestApplication.Data;
using WpfTestApplication.Data.AdventureWorks2014DataSetTableAdapters;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : ItemsViewModel
    {
        protected override void LoadData()
        {
            AdventureWorks2014DataSet adventureWorksDataSet = new AdventureWorks2014DataSet();

            ProductTableAdapter productsTableAdapter = new ProductTableAdapter();
            productsTableAdapter.Fill(adventureWorksDataSet.Product);

            Items = adventureWorksDataSet.Product;
        }
    }
}
