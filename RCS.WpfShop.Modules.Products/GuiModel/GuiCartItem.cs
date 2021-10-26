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

            SetValue();
        }

        /// <summary>
        /// Local copy, as a reference into the repository is not possible. 
        /// </summary>
        public CartItem CartItem { get; }

        private void SetValue()
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

                // Need this because this is no DependencyProperty, as I also want to use CartItem.Quantity instead of duplicating it.
                RaisePropertyChanged(nameof(Quantity));

                SetValue();
            }
        }

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(GuiCartItem));

        public decimal Value
        {
            get => (decimal)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion

        // TODO Make this a ViewModel?
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Currently not used.
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}