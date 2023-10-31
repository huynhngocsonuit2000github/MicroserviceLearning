using Basket.Api.Data;
using Basket.Api.Entity;
using Basket.Api.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Basket.Api.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IBasketContext _context;

        public CartRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            return await _context
                            .Carts
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<Cart> GetCart(string id)
        {
            return await _context
                           .Carts
                           .Find(e => e.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Cart> GetCartByUserId(string userId)
        {
            return await _context
                           .Carts
                           .Find(e => e.UserId == userId)
                           .FirstOrDefaultAsync();
        }

        public async Task CreateCart(Cart cart)
        {
            await _context.Carts.InsertOneAsync(cart);
        }
        public async Task<bool> UpdateCart(Cart cart)
        {
            var updateResult = await _context
                                        .Carts
                                        .ReplaceOneAsync(filter: g => g.Id == cart.Id, replacement: cart);
            return updateResult.IsAcknowledged
                                && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteCart(string id)
        {
            FilterDefinition<Cart> filter = Builders<Cart>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                            .Carts
                                                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                            && deleteResult.DeletedCount > 0;
        } 
    }
}
