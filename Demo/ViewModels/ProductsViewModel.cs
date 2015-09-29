using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
using Demo.Views;
using Demo.Windows;
using Microsoft.Practices.Prism.Commands;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Demo.ViewModels
{
    class ProductsViewModel : FilterItemsViewModel, IShopper
    {
        public override string MasterFilterSelectedValuePath { get { return "ProductCategoryID"; } }
        public override string DetailFilterSelectedValuePath { get { return "ProductSubcategoryID"; } }
        public override string DetailFilterMasterKeyPath { get { return "ProductCategoryID"; } }

        // TODO This should become parameterized, currently it assumes retrieval of an entire table.
        public override async void Refresh()
        {
            // TODO Possibly maintain static collections in ShoppingWrapper again.
            // TODO Check for errors.
            Items = await ShoppingWrapper.Instance.GetProductsOverview();
        }

        protected override async void SetFilters()
        {
            // TODO Possibly maintain static collections in ShoppingWrapper again.
            var getProductCategories = ShoppingWrapper.Instance.GetProductCategories();
            var getProductSubcategories = ShoppingWrapper.Instance.GetProductSubcategories();

            await Task.WhenAll(getProductCategories, getProductSubcategories);

            // Make sure that both Categories and Subcategories have been retrieved.
            MasterFilterItems = getProductCategories.Result;
            DetailFilterItems = getProductSubcategories.Result;

            // Note that MasterFilterValue also determines the filter on DetailFilterItems.
            MasterFilterValue = NoId;
            DetailFilterValue = NoId;
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        protected override void ShowDetails(object parameter)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            View productView = new ProductView() { ViewModel = productViewModel };

            OkWindow productWindow = new OkWindow() { View = productView, };
            // TODO Make the type of the Item and Items explicit for this model by a parameter?
            productWindow.SetBinding(Window.TitleProperty, new Binding("Item.Name") { Source = productViewModel });
            productWindow.Show();

            productViewModel.Refresh((int)parameter);
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.CartProduct((int)parameter);
        }

        public override object NoId { get { return ShoppingWrapper.Instance.NoId; } }
    }
}
