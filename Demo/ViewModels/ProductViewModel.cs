using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using ProductDetailsRow = Demo.Model.ProductsDataSet.ProductDetailsRow;

namespace Demo.ViewModels
{
    class ProductViewModel : ItemViewModel<ProductDetailsRow>, IShopper
    {
        public override object NoId { get { return ShoppingWrapper.Instance.NoId; } }

        protected override object GetItemId()
        {
            return Item != null ? Item.ProductID : NoId;
        }

        public override async void Refresh(object productId)
        {
            // TODO Check for errors.
            Item = await ShoppingWrapper.Instance.GetProductDetails((int)productId);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<object>(CartProduct);
        }

        public ICommand CartCommand { get; set; }

        private void CartProduct(object parameter)
        {
            ShoppingCartViewModel.Instance.CartProduct((int)parameter);
        }
    }
}