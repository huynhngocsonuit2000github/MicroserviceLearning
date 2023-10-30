using Discounts.Api.Data;
using Discounts.Api.Entity;
using MongoDB.Driver;

namespace Discounts.Api.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDiscountContext _context;
        public DiscountRepository(IDiscountContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Discount>> GetDiscounts()
        {
            return await _context
                            .Discounts
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<Discount> GetDiscount(string id)
        {
            return await _context
                           .Discounts
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }
        public async Task<Discount> GetDiscountByProductId(string productId)
        {
            return await _context
                            .Discounts
                            .Find(e => e.ProductId == productId)
                            .FirstOrDefaultAsync();
        }
        public async Task CreateDiscount(Discount discount)
        {
            await _context.Discounts.InsertOneAsync(discount);
        }
        public async Task<bool> UpdateDiscount(Discount discount)
        {
            var updateResult = await _context
                                        .Discounts
                                        .ReplaceOneAsync(filter: g => g.Id == discount.Id, replacement: discount);
            return updateResult.IsAcknowledged
                                && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteDiscount(string id)
        {
            FilterDefinition<Discount> filter = Builders<Discount>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                            .Discounts
                                                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                            && deleteResult.DeletedCount > 0;
        }
    }
}
