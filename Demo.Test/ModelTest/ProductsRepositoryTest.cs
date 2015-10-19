using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Demo.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest : ModelTest
    {
        [TestMethod()]
        public async Task ReadOverviewTest()
        {
            var target = ProductsRepository.Instance;
            var dto = ProductsOverviewRowDto(1, target.NoId);

            target.Clear();
            target.CreateOverviewProduct(dto);

            // TODO This calls for a service client.
            var result = await ProductsRepository.Instance.ReadOverview();

            Assert.AreEqual(1, result.Count);
        }
    }
}