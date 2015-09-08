using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Input;
using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
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

        public override void Refresh(object id)
        {
            ShoppingWrapper.Instance.BeginGetProductDetails(id, new RunWorkerCompletedEventHandler(GetItemCompleted));
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