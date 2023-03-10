using static RCS.WpfShop.AdventureWorks.Wcf.ProductsServiceClient;

namespace RCS.WpfShop.AdventureWorks.Wrappers
{
    // Container class.
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
