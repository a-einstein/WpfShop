using RCS.AdventureWorks.Common.DomainClasses;
using System;

namespace RCS.WpfShop.Modules.Products.Test.BaseClasses
{
    public abstract class ModelTest
    {
        // Convenience method.
        protected static ProductsOverviewObject ProductsOverviewObject(int id)
        {
            var instance = new ProductsOverviewObject()
            {
                Color = "a Color",
                ListPrice = id,
                Name = Format("Name", id),
                ProductCategory = Format("ProductCategory", id),
                Id = id,
                ProductSubcategory = Format("ProductSubcategory", id),
                Size = id.ToString(),
                SizeUnitMeasureCode = "SUM",
                ThumbNailPhoto = Array.Empty<byte>(),
                WeightUnitMeasureCode = "WUM"
            };

            return instance;
        }

        public static string Format(string aString, int anInt)
        {
            return $"{aString} {anInt}";
        }
    }
}
