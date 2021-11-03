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
            // Note this does not use the mocked data yet.
            // Need to accommodate IFilterRepository.
            await ListTestAsync(0);
        }
    }
}