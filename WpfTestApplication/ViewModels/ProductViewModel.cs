using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using ProductDetailsRow = WpfTestApplication.Model.ProductsDataSet.ProductDetailsRow;

namespace WpfTestApplication.ViewModels
{
    class ProductViewModel : ItemViewModel<ProductDetailsRow>, IShopper
    {
        protected override void LoadData()
        {
            Item = ShoppingWrapper.Instance.ProductDetails(ItemId);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.AddProduct((int)parameter);
        }
    }
}