using Common.DomainClasses;

namespace Demo.Modules.Products.Model.Test
{
    public abstract class ModelTest
    {
        // Convenience method.
        public static ProductsOverviewObject ProductsOverviewObject(int id)
        {
            var instance = new ProductsOverviewObject()
            {
                Color = "a Color",
                ListPrice = (decimal)id,
                Name = Format("Name", id),
                ProductCategory = Format("ProductCategory", id),
                Id = id,
                ProductSubcategory = Format("ProductSubcategory", id),
                Size = id.ToString(),
                SizeUnitMeasureCode = "SUM",
                ThumbNailPhoto = new byte[0],
                WeightUnitMeasureCode = "WUM"
            };

            return instance;
        }

        public static string Format(string aString, int anInt)
        {
            return string.Format("{0} {1}", aString, anInt);
        }
    }
}
