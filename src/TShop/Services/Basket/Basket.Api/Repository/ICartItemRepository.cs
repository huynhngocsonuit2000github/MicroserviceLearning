using Basket.Api.Entity;

namespace Basket.Api.Repository
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItems();
        Task<CartItem> GetCartItem(string id);
        Task<IEnumerable<CartItem>> GetCartItemByCartId(string cartId);
        Task<CartItem> GetCartItemByProductId(string productId);
        Task CreateCartItem(CartItem cartItem);
        Task<bool> UpdateCartItem(CartItem cartItem);
        Task<bool> DeleteCartItem(string id);
    }
}
