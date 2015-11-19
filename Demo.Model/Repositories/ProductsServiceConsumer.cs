using Demo.ServiceClients.Products.ServiceReference;

namespace Demo.Model
{
    public abstract class ProductsServiceConsumer
    {
        public int NoId { get { return -1; } }

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
