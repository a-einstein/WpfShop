using System.Data;
using System.Diagnostics;

namespace Demo.Model.DataSet
{
    // Note that nullable properties are changed in the xsd to have NullValue to be 'Null' instead of 'Throw exception'. 
    // For value types like int a true value may have been chosen, like 0.
    // It is done by hand. There might be some setting to generate that standardly, but only in paid versions of VS.
    // Strangely, setting ProductDetailsRow.LargePhoto to have NullValue to be 'Empty' made the property disappear altogether.
    // This did bot occur with ProductsOverviewRow.ThumbNailPhoto.
    // This has been done to prevent StrongTypingException.
    // The exception may not occur anyway, as it was suppressed after an occurance. 
    // In VS2015 Express I have not seen an explicit setting to reset that, while the general control of exceptions that was present in VS2013 Express seems to have been taken out.

    partial class ProductsDataSet
    {
        [DebuggerDisplay("{ProductID}, {Name}")]
        public partial class ProductsOverviewRow { }

        public partial class ProductsOverviewDataTable
        {
            public ProductsOverviewRow NewRow(
                int productID,
                string Name,
                string Color,
                decimal ListPrice,
                string Size,
                string SizeUnitMeasureCode,
                string WeightUnitMeasureCode,
                byte[] ThumbNailPhoto,
                int productCategoryID,
                string ProductCategory,
                int productSubcategoryID,
                string ProductSubcategory)
            {
                ProductsOverviewRow newRow = ((ProductsOverviewRow)(NewRow()));

                // HACK Because there was no full version of this function and direct assignment to the missing properties was not allowed.
                // TODO Check how to determine certain order of parameters.
                object[] columnValuesArray = new object[] {
                        productID,
                        Name,
                        Color,
                        ListPrice,
                        Size,
                        SizeUnitMeasureCode,
                        WeightUnitMeasureCode,
                        ThumbNailPhoto,
                        productCategoryID,
                        ProductCategory,
                        productSubcategoryID,
                        ProductSubcategory};

                newRow.ItemArray = columnValuesArray;

                return newRow;
            }
        }

        [DebuggerDisplay("{ProductID}, {Name}")]
        public partial class ProductDetailsRow { }

        public partial class ProductDetailsDataTable
        {
            public ProductDetailsRow NewRow(
                int productID,
                string Name,
                decimal ListPrice,
                string Color,
                string Size,
                string SizeUnitMeasureCode,
                decimal Weight,
                string WeightUnitMeasureCode,
                byte[] LargePhoto,
                string ModelName,
                string Description,
                int productCategoryID,
                string ProductCategory,
                int productSubcategoryID,
                string ProductSubcategory)
            {
                ProductDetailsRow newRow = ((ProductDetailsRow)(NewRow()));

                // HACK Because there was no full version of this function and direct assignment to the missing properties was not allowed.
                // TODO Check how to determine certain order of parameters.
                object[] columnValuesArray = new object[] {
                        Name,
                        ListPrice,
                        Color,
                        Size,
                        SizeUnitMeasureCode,
                        Weight,
                        WeightUnitMeasureCode,
                        LargePhoto,
                        ModelName,
                        Description,
                        productID,
                        productCategoryID,
                        ProductCategory,
                        productSubcategoryID,
                        ProductSubcategory};

                newRow.ItemArray = columnValuesArray;

                return newRow;
            }
        }

        [DebuggerDisplay("{ProductCategoryID}, {Name}")]
        public partial class ProductCategoriesRow { }

        public partial class ProductCategoriesDataTable
        {
            public ProductCategoriesRow NewRow(
                int ProductCategoryID,
                string Name)
            {
                ProductCategoriesRow newRow = ((ProductCategoriesRow)(NewRow()));

                // HACK Because there was no full version of this function and direct assignment to the missing properties was not allowed.
                // TODO Check how to determine certain order of parameters.
                object[] columnValuesArray = new object[] {
                        ProductCategoryID,
                        Name};

                newRow.ItemArray = columnValuesArray;

                return newRow;
            }
        }

        [DebuggerDisplay("{ProductCategoryID}, {ProductSubcategoryID}, {Name}")]
        public partial class ProductSubcategoriesRow { }

        public partial class ProductSubcategoriesDataTable
        {
            public ProductSubcategoriesRow NewRow(
                int ProductSubcategoryID,
                string Name,
                int ProductCategoryID)
            {
                ProductSubcategoriesRow newRow = ((ProductSubcategoriesRow)(NewRow()));

                // HACK Because there was no full version of this function and direct assignment to the missing properties was not allowed.
                // TODO Check how to determine certain order of parameters.
                object[] columnValuesArray = new object[] {
                        ProductSubcategoryID,
                        Name,
                        ProductCategoryID
                };

                newRow.ItemArray = columnValuesArray;

                return newRow;
            }
        }

        [DebuggerDisplay("{Count} rows")]
        public partial class ShoppingCartItemsDataTable
        {
            public DataRow[] GetByProductId(int productId)
            {
                var productQuery = string.Format("ProductID = {0}", productId);
                var cartItems = Select(productQuery);

                return cartItems;
            }
        }

        [DebuggerDisplay("{ProductName}, {Quantity}, {ProductListPrice}, {Value}")]
        public partial class ShoppingCartItemsRow
        { }
    }
}
