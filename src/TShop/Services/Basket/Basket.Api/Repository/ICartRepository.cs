using Basket.Api.Entity;
using MongoDB.Bson;

namespace Basket.Api.Repository
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCarts();
        Task<Cart> GetCart(string id);
        Task<Cart> GetCartByUserId(string userId);
        Task CreateCart(Cart cartItem);
        Task<bool> UpdateCart(Cart cartItem);
        Task<bool> DeleteCart(string id);
    }
}
