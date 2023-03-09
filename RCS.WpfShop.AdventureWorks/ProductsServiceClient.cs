namespace RCS.WpfShop.AdventureWorks.Wcf
{
    public partial class ProductsServiceClient
    {
        // Intended to enable injection.
        // TODO Maybe the constructor with Binding could be applied instead with more options.
        public ProductsServiceClient(EndpointAndAddressConfiguration configuration)
            : this(configuration.EndpointConfiguration, configuration.RemoteAddress)
        { }

        public class EndpointAndAddressConfiguration
        {
            public EndpointAndAddressConfiguration(EndpointConfiguration endpointConfiguration, string remoteAddress)
            {
                EndpointConfiguration = endpointConfiguration;
                RemoteAddress = remoteAddress;
            }

            public EndpointConfiguration EndpointConfiguration { get; }

            public string RemoteAddress { get; }
        }
    }
}
