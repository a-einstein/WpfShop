using Common.DomainClasses;

namespace Demo.Model.Test
{
    public abstract class ModelTest
    {
        public static ProductsOverviewObject ProductsOverviewObject(int id, object noId)
        {
            var instance = new ProductsOverviewObject()
            {
                Color = "a Color",
                ListPrice = (decimal)id,
                Name = Format("Name", id),
                ProductCategory = Format("ProductCategory", id),
                ProductCategoryId = (int)noId,
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
