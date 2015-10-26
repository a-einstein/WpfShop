using Demo.Model.DataSet;
using System;

namespace Demo.Model
{
    // Note that ProductsDataSet cannot be a singleton. That has been a motivation to create this wrapper.
 
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
        internal static ShoppingWrapper Instance
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

        // Make this internal to hide the actual data cache.
        // That has been the motivation to make this a separate project.
        internal ProductsDataSet ProductsDataSet { get { return productsDataSet; } }
    }
}
