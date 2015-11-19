using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
        public void ListTest()
        {
            var target = ProductsRepository.Instance;
            var element = ProductsOverviewObject(1, target.NoId);

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }
    }
}