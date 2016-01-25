using Common.DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Modules.Products.Model.Test
{
    [TestClass()]
    public class CartItemsRepositoryTest : ModelTest
    {
        [TestMethod()]
        public void CombinationTest()
        {
            var products = ProductsRepository.Instance;
            products.Clear();

            int productId1 = 1;
            decimal price1 = 10;
            var product1 = ProductsOverviewObject(productId1, price1, products.NoId);
            products.List.Add(product1);

            int productId2 = 2;
            decimal price2 = 20;
            var product2 = ProductsOverviewObject(productId2, price2, products.NoId);
            products.List.Add(product2);

            var target = CartItemsRepository.Instance;

            var cartItem1 = target.AddProduct(product1);
            Assert.AreEqual(1, target.ProductsCount());
            Assert.AreEqual(price1, target.CartValue());

            var cartItem2 = target.AddProduct(product2);
            Assert.AreEqual(2, target.ProductsCount());
            Assert.AreEqual(price1 + price2, target.CartValue());

            target.AddProduct(product2);
            Assert.AreEqual(3, target.ProductsCount());
            Assert.AreEqual(price1 + 2 * price2, target.CartValue());

            target.DeleteProduct(cartItem1);
            Assert.AreEqual(2, target.ProductsCount());
            Assert.AreEqual(2 * price2, target.CartValue());
        }

        // Overloaded to clarify price.
        public static ProductsOverviewObject ProductsOverviewObject(int dtoId, decimal listPrice, object noId)
        {
            var instance = ProductsOverviewObject(dtoId, noId);
            instance.ListPrice = listPrice;

            return instance;
        }
    }
}