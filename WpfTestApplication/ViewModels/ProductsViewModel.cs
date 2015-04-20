using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data;
using WpfTestApplication.Data.AdventureWorks2014DataSetTableAdapters;
using WpfTestApplication.Views;

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

        protected override void ShowDetails(object p)
        {
            Window detailWindow = new ProductView();

            // TODO Get the Item or an Id to retrieve it. How?
            detailWindow.Show();
        }
    }
}
