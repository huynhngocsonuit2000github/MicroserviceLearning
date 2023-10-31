using Basket.Api.Data;
using Basket.Api.Entity;
using Basket.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Basket.Api.Data
{
    public class BasketContext : IBasketContext
    {
        public BasketContext(IOptions<BasketDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            CartItems = database.GetCollection<CartItem>(settingValue.CartItemsCollectionName);
            Carts = database.GetCollection<Cart>(settingValue.CartsCollectionName);
        }

        public IMongoCollection<CartItem> CartItems { get; }
        public IMongoCollection<Cart> Carts { get; }
    }
}
