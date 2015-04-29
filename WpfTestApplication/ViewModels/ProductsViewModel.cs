using System.Data;
using System.Windows;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Data.ProductsDataSetTableAdapters;
using WpfTestApplication.Views;
using ProductCategoryDataTable = WpfTestApplication.Data.ProductsDataSet.ProductCategoryDataTable;
using ProductsOverviewDataTable = WpfTestApplication.Data.ProductsDataSet.ProductsOverviewDataTable;
using ProductsOverviewRow = WpfTestApplication.Data.ProductsDataSet.ProductsOverviewRow;
using ProductSubcategoryDataTable = WpfTestApplication.Data.ProductsDataSet.ProductSubcategoryDataTable;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : ItemsViewModel
    {
        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        protected override void LoadData()
        {
            ProductsOverviewTableAdapter productsTableAdapter = new ProductsOverviewTableAdapter();
            // Note this currently takes in all the table data. Of course this should be prefiltered and/or paged in a realistic environment. 
            ProductsOverviewDataTable productsOverviewTable = productsTableAdapter.GetData();
            Items = productsOverviewTable;

            ProductCategoryTableAdapter categoriesTableAdapter = new ProductCategoryTableAdapter();
            ProductCategoryDataTable categoriesTable = new ProductCategoryDataTable();
            categoriesTable.AddProductCategoryRow(string.Empty);
            categoriesTableAdapter.ClearBeforeFill = false;
            categoriesTableAdapter.Fill(categoriesTable);
            MasterFilterItems = categoriesTable;

            ProductSubcategoryTableAdapter subcategoriesTableAdapter = new ProductSubcategoryTableAdapter();
            ProductSubcategoryDataTable subcategoriesTable = new ProductSubcategoryDataTable();
            subcategoriesTable.AddProductSubcategoryRow(string.Empty, categoriesTable.FindByProductCategoryID(-1));
            subcategoriesTableAdapter.ClearBeforeFill = false;
            subcategoriesTableAdapter.Fill(subcategoriesTable);
            DetailFilterItems = subcategoriesTable;

            // Note that MasterFilterValue also determines DetailFilterItems.
            MasterFilterValue = -1;
            DetailFilterValue = -1;
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
