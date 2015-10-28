using System;
using System.Data;
using ShoppingCartItemsDataTable = Demo.Model.DataSet.ProductsDataSet.ShoppingCartItemsDataTable;
using ShoppingCartItemsRow = Demo.Model.DataSet.ProductsDataSet.ShoppingCartItemsRow;
using ShoppingCartsDataTable = Demo.Model.DataSet.ProductsDataSet.ShoppingCartsDataTable;
using ShoppingCartsRow = Demo.Model.DataSet.ProductsDataSet.ShoppingCartsRow;

namespace Demo.Model
{
    public class CartItemsRepository
    {
        private CartItemsRepository()
        { }

        private static volatile CartItemsRepository instance;
        private static object syncRoot = new Object();

        public static CartItemsRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CartItemsRepository();
                    }
                }

                return instance;
            }
        }

        // TODO This table might be removed alltogether, as only one cart is used. Then the current relation and non nullable key should be remove, or made nullable.
        private ShoppingCartsDataTable Carts
        {
            get
            {
                return ShoppingWrapper.Instance.ProductsDataSet.ShoppingCarts;
            }
        }

        private ShoppingCartsRow cart;

        private ShoppingCartsRow Cart
        {
            get
            {
                if (cart == null)
                {
                    const string cartId = "1";

                    Carts.AddShoppingCartsRow(cartId);
                    Carts.AcceptChanges();

                    cart = Carts.Rows.Find(cartId) as ShoppingCartsRow;
                }

                return cart;
            }
        }

        // Note that simply the whole table is used, as all items belong to the user.
        // It is only kept in memory and not preserved. It is anticipated that only real orders are preserved.
        private ShoppingCartItemsDataTable CartItems { get { return ShoppingWrapper.Instance.ProductsDataSet.ShoppingCartItems; } }

        public DataView Items { get { return CartItems.DefaultView; } }

        private const string cartItemsNumberExceptionMessage = "Unexpected number of found ShoppingCartItems.";
        private const string productNotFoundExceptionMessage = "Product not found.";

        public void AddProduct(int productId)
        {
            DataRow[] existingCartItems = CartItems.GetByProductId(productId);

            if (existingCartItems.Length == 0)
            {
                var now = DateTime.Now;
                var productRow = ShoppingWrapper.Instance.ProductsDataSet.ProductsOverview.FindByProductID((int)productId);

                if (productRow != null)
                {
                    // Note that ShoppingCartId currently is non nullable.
                    var cartItem = CartItems.AddShoppingCartItemsRow(Cart, 1, productRow, now, now);

                    CartItems.AcceptChanges();
                }
                else
                    throw new Exception(productNotFoundExceptionMessage);
            }
            else if (existingCartItems.Length == 1)
            {
                var cartItem = existingCartItems[0] as ShoppingCartItemsRow;
                cartItem.Quantity += 1;

                cartItem.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public void DeleteProduct(int productId)
        {
            // It would be more convenient if ProductId was the key. Is more logical and makes it easy to get the cartItem by FindByProductID. 
            // Unfortunately this is table is linked to an actual table in the DB which has key ShoppingCartItemID.
            DataRow[] existingCartItems = CartItems.GetByProductId(productId);

            if (existingCartItems.Length == 1)
            {
                var cartItem = existingCartItems[0] as ShoppingCartItemsRow;

                cartItem.Delete();
                CartItems.AcceptChanges();
            }
            else
                throw new Exception(cartItemsNumberExceptionMessage);
        }

        public int ProductsCount()
        {
            return CartItems.Count > 0
            ? Convert.ToInt32(CartItems.Compute("Sum(Quantity)", null))
            : 0;
        }

        public Decimal CartValue()
        {
            return CartItems.Count > 0
            ? Convert.ToDecimal(CartItems.Compute("Sum(Value)", null))
            : 0;
        }
    }
}
