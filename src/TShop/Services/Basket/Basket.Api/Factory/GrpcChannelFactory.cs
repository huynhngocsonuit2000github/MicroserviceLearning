using Grpc.Net.Client;

namespace Basket.Api.Factory
{ 
    public enum GrpcChannelServer
    {
        Catalog,
        Discount
    }
    public class GrpcChannelFactory : IGrpcChannelFactory
    {
        private readonly IConfiguration _configuration;

        public GrpcChannelFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public GrpcChannel GetGrpcChannel(GrpcChannelServer grpcChannelServer)
        {
            var urlServer = grpcChannelServer switch
            {
                GrpcChannelServer.Catalog => "GrpcService:Catalog:CatalogApiUrl",
                GrpcChannelServer.Discount => "GrpcService:Discount:DiscountApiUrl",
                _ => throw new NotImplementedException(),
            };

            var address = _configuration.GetSection(urlServer)!.Value!.ToString()!;
            var options = new GrpcChannelOptions()
            {
                HttpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
            };

            var channel = GrpcChannel.ForAddress(address, options);
            return channel;


        }
    }
}
