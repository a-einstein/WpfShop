using System.Diagnostics;

namespace ServiceClients.Products.ServiceReference
{
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductsOverviewObject { }

    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class Product { }

    [DebuggerDisplay("{ProductCategoryID}, {Name}")]
    public partial class ProductCategory { }

    [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
    public partial class ProductSubcategory { }
}