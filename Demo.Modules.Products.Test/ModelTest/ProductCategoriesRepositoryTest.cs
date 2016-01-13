using Common.DomainClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductCategoriesRepositoryTest
    {
        [TestMethod()]
        public void ListTest()
        {
            var target = ProductCategoriesRepository.Instance;
            var element = ProductCategory(target.NoId);

            target.Clear();
            target.List.Add(element);

            var result = target.List.Count;

            Assert.AreEqual(1, result);
        }

        public static ProductCategory ProductCategory(object noId)
        {
            var instance = new ProductCategory()
            {
                Name = "a Name",
                Id = (int)noId
            };

            return instance;
        }
    }
}