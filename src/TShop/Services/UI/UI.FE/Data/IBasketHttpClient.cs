using UI.FE.Models;

namespace UI.FE.Data
{
    public interface IBasketHttpClient
    {
        Task AddToCart(CartItemAddingRequest request);
        Task<CartResponse?> GetCartByUserIdAsync(string userId);
    }
}