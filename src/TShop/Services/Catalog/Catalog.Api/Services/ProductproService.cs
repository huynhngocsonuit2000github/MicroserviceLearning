using Catalog.Api.Repository;
using Grpc.Core;
using MongoDB.Bson;

namespace Catalog.Api.Services
{
    public class ProductproService : Productpro.ProductproBase
    {
        private readonly ILogger<ProductproService> _logger;
        private readonly IProductRepository _productRepository;
        public ProductproService(ILogger<ProductproService> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public override async Task<GetByIdResponsepro> GetById(GetByIdRequestpro request, ServerCallContext context)
        {
            _logger.LogInformation("==>> Start GetById: " + request.ToJson());

            var product = await _productRepository.GetProduct(request.Id);

            var productPro = new GetByIdResponsepro()
            {
                Id = product.Id,
                CategoryName = product.CategoryName,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Name = product.Name,
                Price = (double)product.Price,
                Summary = product.Summary,
            };

            return productPro;
        }
    }
}
