using Demo.ServiceClients.Products.ServiceReference;

namespace Demo.Model
{
    public abstract class ProductsServiceConsumer
    {
        // TODO Decide where to put this.
        public int NoId { get { return ShoppingWrapper.NoId; } }

        private ProductsServiceClient productsServiceClient;

        protected ProductsServiceClient ProductsServiceClient
        {
            get
            {
                if (productsServiceClient == null)
                    productsServiceClient = new ProductsServiceClient();

                return productsServiceClient;
            }
        }
    }
}
