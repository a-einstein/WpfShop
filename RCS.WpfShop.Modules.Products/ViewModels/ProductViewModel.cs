using Prism.Commands;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Common.ViewModels;
using RCS.WpfShop.Common.Views;
using RCS.WpfShop.Common.Windows;
using RCS.WpfShop.Modules.Products.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RCS.WpfShop.Modules.Products.ViewModels
{
    public class ProductViewModel : ItemViewModel<Product>, IShopper
    {
        #region Refresh
        protected override void SetCommands()
        {
            base.SetCommands();

            CartCommand = new DelegateCommand<Product>(CartProduct);
            PhotoCommand = new DelegateCommand(ShowPhoto);
        }

        private bool itemRead;

        protected override async Task<bool> Read()
        {
            // Assume the Item only needs to be read successfully once, 
            // to avoid an unnecessary read when opening the photo.
            if (!itemRead && ItemId.HasValue)
            {
                var result = await ProductsRepository.Instance.ReadDetails((int)ItemId);
                itemRead = result != null;
                Item = result;
            }

            return itemRead;
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
            get => (ICommand)GetValue(CartCommandProperty);
            set => SetValue(CartCommandProperty, value);
        }

        private void CartProduct(Product product)
        {
            ShoppingCartViewModel.Instance.CartProduct(product);
        }
        #endregion

        #region Photo
        public static readonly DependencyProperty PhotoCommandProperty =
              DependencyProperty.Register(nameof(PhotoCommand), typeof(ICommand), typeof(ProductViewModel));

        public ICommand PhotoCommand
        {
            get => (ICommand)GetValue(PhotoCommandProperty);
            private set => SetValue(PhotoCommandProperty, value);
        }

        protected void ShowPhoto()
        {
            // Note that this view and the same picture is reused, which currently seem to have no dimension larger than 240.
            // So enlarging currently has no real advantage.
            // In case of another larger version being stored in the database, that should be retrieved.

            var view = new PhotoView() { ViewModel = this };
            var window = new OkWindow() { View = view };

            window.Show();
        }
        #endregion
    }
}