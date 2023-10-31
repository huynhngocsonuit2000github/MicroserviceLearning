namespace Basket.Api.SyncData
{
    public interface IDiscountproGrpc
    {
        Task<GetByProductIdResponsepro> GetDiscountByProductIdAsync(string productId);
    }
}