using Discounts.Api.Repository;
using Grpc.Core;
using MongoDB.Bson;

namespace Discounts.Api.Services
{
    public class DiscountproService : Discountpro.DiscountproBase
    {
        private readonly ILogger<DiscountproService> _logger;
        private readonly IDiscountRepository _discountRepository;

        public DiscountproService(ILogger<DiscountproService> logger, IDiscountRepository discountRepository)
        {
            _logger = logger;
            _discountRepository = discountRepository;
        }

        public override async Task<GetByProductIdResponsepro> GetByProductId(GetByProductIdRequestpro request, ServerCallContext context)
        {
            _logger.LogInformation("==>> Start GetByProductId: " + request.ToJson());

            var discount = await _discountRepository.GetDiscountByProductId(request.ProductId);

            if (discount is null) return new GetByProductIdResponsepro();

            var productPro = new GetByProductIdResponsepro()
            {
                Id = discount.Id,
                ProductId = discount.Id,
                Amount = (double)discount.Amount,
                Description = discount.Description,
                ProductName = discount.ProductName
            };

            return productPro;
        }

    }
}
