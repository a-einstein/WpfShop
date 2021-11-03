using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCS.AdventureWorks.Common.DomainClasses;
using RCS.WpfShop.Modules.Products.Test.BaseClasses;
using System.Threading.Tasks;

namespace RCS.WpfShop.Modules.Products.Model.Test
{
    [TestClass()]
    public class ProductsRepositoryTest :
        RepositoryTest<ProductsRepository, ProductsOverviewObject>
    {
        [TestMethod()]
        public override async Task ListTestAsync()
        {
            await ListTestAsync(2);
        }

        // TODO This could be part of a future base FilterRepositoryTest.
        // The specific parameters would complicate that.
        // And currently there is only one FilterRepository,
        protected override async Task TestRefresh(int expectedServiceCount, ProductsRepository target)
        {
            // Fill with bare minimum.
            // TODO Better correspondance of expected in and output.
            var category = new ProductCategory() { Id = 2 };
            var subcategory = new ProductSubcategory() { Id = 3 };
            var searchString = "2.3";

            await target.Refresh(category, subcategory, searchString);
            Assert.AreEqual(target.Items.Count, expectedServiceCount);
        }
    }
}