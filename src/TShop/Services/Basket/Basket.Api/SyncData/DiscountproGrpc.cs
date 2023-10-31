using Basket.Api.Factory;
using Grpc.Net.Client;
using MongoDB.Bson;

namespace Basket.Api.SyncData
{
    public class DiscountproGrpc : IDiscountproGrpc
    {
        private readonly GrpcChannel _channel;
        private readonly ILogger<DiscountproGrpc> _logger;

        public DiscountproGrpc(IGrpcChannelFactory channelFactory, ILogger<DiscountproGrpc> logger)
        {
            _channel = channelFactory.GetGrpcChannel(GrpcChannelServer.Discount);
            _logger = logger;
        }

        public async Task<GetByProductIdResponsepro> GetDiscountByProductIdAsync(string productId)
        {
            _logger.LogInformation("==>> Start Calling GetDiscountByProductIdAsync: " + productId);

            var client = new Discountpro.DiscountproClient(_channel);

            var request = new GetByProductIdRequestpro()
            {
                ProductId = productId
            };

            GetByProductIdResponsepro? reply = null;

            try
            {
                _logger.LogInformation("==>> Start Calling GetDiscountByProductIdAsync from Basket to Discount service: " + productId);
                reply = await client.GetByProductIdAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError("Login failed with " + request.ToJson());

                return null!;
            }

            return reply;
        }


    }
}
