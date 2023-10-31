namespace Basket.Api.SyncData
{
    public interface IProductproGrpc
    {
        Task<GetByIdResponsepro> GetByIdAsync(string productId);
    }
}