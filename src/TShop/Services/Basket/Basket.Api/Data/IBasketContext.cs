using Basket.Api.Entity;
using MongoDB.Driver;

namespace Basket.Api.Data
{
    public interface IBasketContext
    {
        IMongoCollection<CartItem> CartItems { get; }
        IMongoCollection<Cart> Carts { get; }
    }
}
