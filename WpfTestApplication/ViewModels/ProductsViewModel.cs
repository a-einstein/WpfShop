using System.Data;
using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data;
using WpfTestApplication.Data.AdventureWorks2014DataSetTableAdapters;
using WpfTestApplication.Views;
using ProductRow = WpfTestApplication.Data.AdventureWorks2014DataSet.ProductRow;

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

        protected override void ShowDetails(object parameter)
        {
            RoutedEventArgs routedEventArgs = parameter as RoutedEventArgs;

            Window productView = new ProductView();

            ProductViewModel productViewModel = productView.DataContext as ProductViewModel;
            productViewModel.Item = ((routedEventArgs.Source as FrameworkElement).DataContext as DataRowView).Row as ProductRow;

            productView.Show();
        }
    }
}
