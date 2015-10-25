using System;

namespace Demo.Model
{
    // TODO Maybe put this functionality into the partial sub classes of ProductsDataSet. But ProductsDataSet could not be a singleton .
    // Other option: make properties here on wrapper sub classes which are instantiated with a single dataset. The constructor should be restricted some way.

    // TODO After having moved data retrieval to a service, on both sides is still hung on to a DataSet, with conversions as a result. That may be optimized.

    // TODO What to do with this class and its contents?

    public class ShoppingWrapper
    {
        private ShoppingWrapper()
        {
            productsDataSet = new ProductsDataSet();
        }

        private static volatile ShoppingWrapper instance;
        private static object syncRoot = new Object();

        // Note this class is a singleton, implemented along the way (but not entirely) of https://msdn.microsoft.com/en-us/library/ff650316.aspx
        public static ShoppingWrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ShoppingWrapper();
                    }
                }

                return instance;
            }
        }

        // Choose for an int as this is the actual type of the Id.
        public static int NoId { get { return -1; } }

        private ProductsDataSet productsDataSet;

        // HACK This should be shared, but not publicly accessible.
        // Move this lot (the Model) to a project an keep this internal?
        public ProductsDataSet ProductsDataSet { get { return productsDataSet; } }
    }
}
