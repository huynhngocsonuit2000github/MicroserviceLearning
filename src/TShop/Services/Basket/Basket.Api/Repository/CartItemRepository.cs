using Basket.Api.Data;
using Basket.Api.Entity;
using MongoDB.Driver;

namespace Basket.Api.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly IBasketContext _context;

        public CartItemRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            return await _context
                            .CartItems
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<CartItem> GetCartItem(string id)
        {
            return await _context
                           .CartItems
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemByCartId(string cartId)
        {
            return await _context
                           .CartItems
                           .Find(e => e.CartId == cartId)
                           .ToListAsync();
        }

        public async Task CreateCartItem(CartItem product)
        {
            await _context.CartItems.InsertOneAsync(product);
        }
        public async Task<bool> UpdateCartItem(CartItem product)
        {
            var updateResult = await _context
                                        .CartItems
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                                && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteCartItem(string id)
        {
            FilterDefinition<CartItem> filter = Builders<CartItem>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                            .CartItems
                                                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                            && deleteResult.DeletedCount > 0;
        }

        public async Task<CartItem> GetCartItemByProductId(string productId)
        {
            return await _context
                           .CartItems
                           .Find(p => p.ProductId == productId)
                           .FirstOrDefaultAsync();
        }
    }
}
