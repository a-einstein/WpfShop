using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
        public void ListTest()
        {
            // Note that injection is not possible in test classes.
            // "Test classes need to have an empty default constructor or no constructors at all."
            var target = new ProductsRepository();
            var element = ProductsOverviewObject(1);

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }
    }
}