using Basket.Api.Factory;
using Grpc.Net.Client;
using MongoDB.Bson;

namespace Basket.Api.SyncData
{
    public class ProductproGrpc : IProductproGrpc
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<ProductproGrpc> _logger;

        public ProductproGrpc(IGrpcChannelFactory channelFactory, ILogger<ProductproGrpc> logger)
        {
            _channel = channelFactory.GetGrpcChannel(GrpcChannelServer.Catalog);
            _logger = logger;
        }

        public async Task<GetByIdResponsepro> GetByIdAsync(string productId)
        {
            _logger.LogInformation("==>> Start Calling GetByIdAsync: " + productId);

            var client = new Productpro.ProductproClient(_channel);

            var request = new GetByIdRequestpro()
            {
                Id = productId
            };

            GetByIdResponsepro? reply = null;

            try
            {
                _logger.LogInformation("==>> Start Calling GetByIdAsync from Basket to Product service: " + productId);
                reply = await client.GetByIdAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError("GetByIdAsync failed with " + request.ToJson());

                return null!;
            }

            return reply;
        }


    }
}
