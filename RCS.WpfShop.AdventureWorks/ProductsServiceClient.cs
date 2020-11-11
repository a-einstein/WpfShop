namespace RCS.WpfShop.AdventureWorks.ServiceReferences
{
    public partial class ProductsServiceClient
    {
        // Intended to enable injection.
        // TODO Maybe the constructor with Binding could be applied instead with more options.
        public ProductsServiceClient(EndpointAndAddressConfiguration configDinges)
            : this(configDinges.EndpointConfiguration, configDinges.RemoteAddress)
        { }

        public class EndpointAndAddressConfiguration
        {
            public EndpointAndAddressConfiguration(EndpointConfiguration endpointConfiguration, string remoteAddress)
            {
                EndpointConfiguration = endpointConfiguration;
                RemoteAddress = remoteAddress;
            }

            public EndpointConfiguration EndpointConfiguration { get; private set; }

            public string RemoteAddress { get; private set; }
        }
    }
}
