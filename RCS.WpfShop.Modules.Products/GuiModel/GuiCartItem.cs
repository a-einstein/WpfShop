using RCS.AdventureWorks.Common.DomainClasses;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace RCS.WpfShop.Modules.Products.GuiModel
{
    /// <summary>
    /// Wrapper around CartItem.
    /// </summary>
    [DebuggerDisplay("{Name} : {ProductListPrice} x {Quantity} = {Value}")]
    public class GuiCartItem : DependencyObject
    {
        #region Construction
        public GuiCartItem(CartItem cartItem)
        {
            CartItem = cartItem;

            // Update binding.
            Quantity = CartItem.Quantity;
        }

        /// <summary>
        /// Local COPY, as a reference into the repository is not possible. 
        /// </summary>
        public CartItem CartItem { get; }

        private void UpdateValue()
        {
            Value = ProductListPrice * Quantity;
        }
        #endregion

        #region Bindable but not updateable.
        public int? Id => CartItem.Id;
        public string Name => CartItem.Name;
        public int ProductId => CartItem.ProductId;
        public string ProductSize => CartItem.ProductSize;
        public string ProductSizeUnitMeasureCode => CartItem.ProductSizeUnitMeasureCode;
        public string ProductColor => CartItem.ProductColor;
        public decimal ProductListPrice => CartItem.ProductListPrice;
        #endregion

        #region Bindable and updateable.
        public int Quantity
        {
            get => CartItem.Quantity;
            set
            {
                CartItem.Quantity = value;

                // Do this before the PropertyChanged, to be available too.
                UpdateValue();

                // Need this because this is no BindableProperty, as I also want to use CartItem.Quantity instead of duplicating it.
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        // TODO Use either of these 2 methods, removing the other.
        // Caused by apparent binding problems on IntegerUpDown.

        /*
        private static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register(nameof(Quantity), typeof(int), typeof(GuiCartItem));

        public int Quantity
        {
            get
            {
                _ = (int)GetValue(QuantityProperty);
                return CartItem.Quantity;
            }
            set
            {
                SetValue(QuantityProperty, value);
                CartItem.Quantity = value;

                UpdateValue();
            }
        }
        */

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(GuiCartItem));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }
        #endregion

        // TODO Make this a ViewModel?
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Note OnPropertyChanged can't be used, as it differs from Xamarin.Forms.BindableObject. 
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}