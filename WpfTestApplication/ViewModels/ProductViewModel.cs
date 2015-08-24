using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows.Input;
using WpfTestApplication.BaseClasses;
using WpfTestApplication.Interfaces;
using WpfTestApplication.Model;
using ProductDetailsRow = WpfTestApplication.Model.ProductsDataSet.ProductDetailsRow;

namespace WpfTestApplication.ViewModels
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