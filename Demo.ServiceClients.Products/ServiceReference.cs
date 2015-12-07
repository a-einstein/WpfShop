using System.Diagnostics;

namespace ServiceClients.Products.ServiceReference
{
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductsOverviewObject { }

    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class Product { }

    [DebuggerDisplay("{ProductCategoryId}, {Name}")]
    public partial class ProductCategory { }

    [DebuggerDisplay("{ProductCategoryId}, {ProductSubcategoryId}, {Name}")]
    public partial class ProductSubcategory { }
}