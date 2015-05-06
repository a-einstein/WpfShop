using Microsoft.Practices.Prism.Commands;
using System.Windows;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using WpfTestApplication.Views;

namespace WpfTestApplication.ViewModels
{
    class ProductsViewModel : FilterItemsViewModel, IShopper
    {
        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        protected override void LoadData()
        {
            Items = ProductsModel.Instance.Products.DefaultView;

            MasterFilterItems = ProductsModel.Instance.ProductCategories;
            DetailFilterItems = ProductsModel.Instance.ProductSubcategories;

            // Note that MasterFilterValue also determines DetailFilterItems.
            MasterFilterValue = -1;
            DetailFilterValue = -1;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        protected override void ShowDetails(object parameter)
        {
            RoutedEventArgs routedEventArgs = parameter as RoutedEventArgs;

            // TODO Generally distinguish between Windows, Pages and Views. 
            // Like
            // - Windows: Main, Details.
            // - Pages: About, Shopping.
            // - Views: Products, Product, ShoppingCart.

            Window productView = new ProductView();

            ProductViewModel productViewModel = productView.DataContext as ProductViewModel;
            productViewModel.ItemId = (int)parameter;

            productView.Show();
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.AddProduct((int)parameter);
        }
    }
}
