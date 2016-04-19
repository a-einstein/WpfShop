using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
        public void ListTest()
        {
            var target = ProductsRepository.Instance;
            var element = ProductsOverviewObject(1);

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }
    }
}