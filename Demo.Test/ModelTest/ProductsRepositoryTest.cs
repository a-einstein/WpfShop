using Demo.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Test.ModelTest
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
         public async void ReadOverviewTest()
        {
            var target = ProductsRepository.Instance;
            var dto = ProductsOverviewRowDto(1, target.NoId);
            target.CreateOverviewProduct(dto);

            var result = await ProductsRepository.Instance.ReadOverview();

            Assert.AreEqual(1, result.Count);
        }
    }
}