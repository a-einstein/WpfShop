using System.Diagnostics;

namespace ServiceClients.Products.ServiceReference
{
    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductsOverviewRowDto { }

    [DebuggerDisplay("{ProductID}, {Name}")]
    public partial class ProductDetailsRowDto { }

    [DebuggerDisplay("{ProductCategoryID}, {Name}")]
    public partial class ProductCategoryRowDto { }

    [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
    public partial class ProductSubcategoryRowDto { }
}