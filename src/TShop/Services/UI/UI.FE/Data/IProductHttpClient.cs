using UI.FE.ResponseDto;

namespace UI.FE.Data
{
    public interface IProductHttpClient
    {
        Task<IEnumerable<Product>?> GetAllProductsAsync();
    }
}