using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Modules.Products.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Construction
        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<Product>(CartProduct);
        }
        #endregion

        #region Refresh
        protected override async Task<bool> Read()
        {
            bool succeeded = false;

            if (ItemId.HasValue)
            {
                var result = await ProductsRepository.Instance.ReadDetails((int)ItemId);
                succeeded = result != null;
                Item = result;
            }

            return succeeded;
        }

        public override string MakeTitle()
        {
            return Item?.Name;
        }
        #endregion

        #region Shopping
        public static readonly DependencyProperty CartCommandProperty =
             DependencyProperty.Register(nameof(CartCommand), typeof(ICommand), typeof(ItemViewModel<Product>));

        // Note this does not work as explicit interface implementation.
        public ICommand CartCommand
        {
            get { return (ICommand)GetValue(CartCommandProperty); }
            set { SetValue(CartCommandProperty, value); }
        }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
        #endregion
    }
}