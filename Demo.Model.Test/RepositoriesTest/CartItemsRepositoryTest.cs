using Demo.ServiceClients.Products.ServiceReference;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Model.Test
{
    [TestClass()]
    public class CartItemsRepositoryTest : ModelTest
    {
        [TestMethod()]
        // Note this conforms to asynchronous tests since VS 2012.
        public void CombinationTest()
        {
            var products = ProductsRepository.Instance;
            products.Clear();

            int productId1 = 1;
            decimal price1 = 10;
            var dto1 = ProductsOverviewRowDto(productId1, price1, products.NoId);
            products.CreateListElement(dto1);

            int productId2 = 2;
            decimal price2 = 20;
            var dto2 = ProductsOverviewRowDto(productId2, price2, products.NoId);
            products.CreateListElement(dto2);

            var target = CartItemsRepository.Instance;

            target.AddProduct(productId1);
            Assert.AreEqual(1, target.ProductsCount());
            Assert.AreEqual(price1, target.CartValue());

            target.AddProduct(productId2);
            Assert.AreEqual(2, target.ProductsCount());
            Assert.AreEqual(price1 + price2, target.CartValue());

            target.AddProduct(productId2);
            Assert.AreEqual(3, target.ProductsCount());
            Assert.AreEqual(price1 + 2*price2, target.CartValue());

            target.DeleteProduct(productId1);
            Assert.AreEqual(2, target.ProductsCount());
            Assert.AreEqual(2*price2, target.CartValue());
        }

        // Overloaded to clarify price.
        public static ProductsOverviewRowDto ProductsOverviewRowDto(int dtoId, decimal listPrice, object noId)
        {
            var dto = ProductsOverviewRowDto(dtoId, noId);
            dto.ListPrice = listPrice;

            return dto;
        }
    }
}