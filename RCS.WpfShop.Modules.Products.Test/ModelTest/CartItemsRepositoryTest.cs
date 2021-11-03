using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Modules.Products.Test.BaseClasses;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class CartItemsRepositoryTest :
        RepositoryTest<CartItemsRepository, CartItem>
    {
        [TestMethod()]
        public override async Task ListTestAsync()
        {
            // Note this does not actually use a service (yet).
            await ListTestAsync(0);
        }

        // TODO Currently this does not really do more than ListTestAsync.
        // Fuctionality should be expanded or transfered.
        // Read the other comments.
        [TestMethod()]
        public async Task CombinationTestAsync()
        {
            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."

            var productId1 = 1;
            decimal price1 = 10;
            var product1 = ProductsOverviewObject(productId1, price1);

            var productId2 = 2;
            decimal price2 = 20;
            var product2 = ProductsOverviewObject(productId2, price2);

            var target = new CartItemsRepository();

            var cartItem1 = new CartItem(product1);
            await target.Create(cartItem1);

            // TODO This would be tests for the viewmodel now.
            //Assert.AreEqual(1, target.ProductsCount());
            //Assert.AreEqual(price1, target.CartValue());

            var cartItem2 = new CartItem(product2);
            await target.Create(cartItem2);

            // TODO This would be tests for the viewmodel now.
            //Assert.AreEqual(2, target.ProductsCount());
            //Assert.AreEqual(price1 + price2, target.CartValue());

            cartItem2.Quantity++;
            await target.Update(cartItem2);

            // TODO The Quantity could be tested by itself through target.Items,
            // but is complicated by the current type of ReadOnlyCollection.
            // It seems it would need to make use of IndexOf, Item[] and
            // add IEqualityComparer to domain classes.

            // TODO This would be tests for the viewmodel now.
            //Assert.AreEqual(3, target.ProductsCount());
            //Assert.AreEqual(price1 + 2 * price2, target.CartValue());

            await target.Delete(cartItem1);

            // TODO This would be tests for the viewmodel now.
            //Assert.AreEqual(2, target.ProductsCount());
            //Assert.AreEqual(2 * price2, target.CartValue());
        }

        // Overloaded to clarify price.
        public static ProductsOverviewObject ProductsOverviewObject(int dtoId, decimal listPrice)
        {
            var instance = ProductsOverviewObject(dtoId);
            instance.ListPrice = listPrice;

            return instance;
        }
    }
}