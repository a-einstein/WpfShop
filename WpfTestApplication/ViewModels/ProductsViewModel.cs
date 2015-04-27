using System.Data;
using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data.ProductsDataSetTableAdapters;
using WpfTestApplication.Views;
using ProductsOverviewDataTable = WpfTestApplication.Data.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = WpfTestApplication.Data.ProductsDataSet.ProductsOverviewRow;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : ItemsViewModel
    {
        protected override void LoadData()
        {
            ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();

            // Note this currently takes in all the table data. Of course this should be prefiltered and/or paged in a realistic environment. 
            ProductsOverviewDataTable productsOverviewDataTable = productsTableAdapter.GetData();

            Items = productsOverviewDataTable;
        }

        protected override void ShowDetails(object parameter)
        {
            RoutedEventArgs routedEventArgs = parameter as RoutedEventArgs;

            Window productView = new ProductView();

            ProductViewModel productViewModel = productView.DataContext as ProductViewModel;
            productViewModel.ItemId = (((routedEventArgs.Source as FrameworkElement).DataContext as DataRowView).Row as ProductsOverviewRow).ProductID;

            productView.Show();
        }
    }
}
