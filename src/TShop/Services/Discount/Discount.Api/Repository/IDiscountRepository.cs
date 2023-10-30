using Discounts.Api.Entity;

namespace Discounts.Api.Repository
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Discount>> GetDiscounts();
        Task<Discount> GetDiscount(string id);
        Task<Discount> GetDiscountByProductId(string productId);
        Task CreateDiscount(Discount product);
        Task<bool> UpdateDiscount(Discount product);
        Task<bool> DeleteDiscount(string id);
    }
}
