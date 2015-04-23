using System.Data;
using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data;
using WpfTestApplication.Data.ProductsOverviewDataSetTableAdapters;
using WpfTestApplication.Views;
using ProductsOverviewRow = WpfTestApplication.Data.ProductsOverviewDataSet.ProductsOverviewRow;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : ItemsViewModel
    {
        protected override void LoadData()
        {
            ProductsOverviewDataSet adventureWorksDataSet = new ProductsOverviewDataSet();

            ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();
            productsTableAdapter.Fill(adventureWorksDataSet.ProductsOverview);

            Items = adventureWorksDataSet.ProductsOverview;
        }

        protected override void ShowDetails(object parameter)
        {
            RoutedEventArgs routedEventArgs = parameter as RoutedEventArgs;

            Window productView = new ProductView();

            ProductViewModel productViewModel = productView.DataContext as ProductViewModel;
            productViewModel.Item = ((routedEventArgs.Source as FrameworkElement).DataContext as DataRowView).Row as ProductsOverviewRow;

            productView.Show();
        }
    }
}
