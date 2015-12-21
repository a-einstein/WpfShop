using Common.DomainClasses;
using Demo.BaseClasses;
using Demo.Interfaces;
using Demo.Model;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace Demo.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product, int>, IShopper
    {
        public override int NoId { get { return ProductsRepository.Instance.NoId; } }

        protected override object GetItemId()
        {
            return Item != null ? Item.Id : NoId;
        }

        public override async void Refresh(object productId)
        {
            // TODO Check for errors.
            Item = await ProductsRepository.Instance.ReadDetails((int)productId);
        }

        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<Product>(CartProduct);
        }

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand { get; set; }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
    }
}