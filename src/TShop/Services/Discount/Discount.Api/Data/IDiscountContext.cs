using Discounts.Api.Entity;
using MongoDB.Driver;

namespace Discounts.Api.Data
{
    public interface IDiscountContext
    {
        IMongoCollection<Discount> Discounts { get; }
    }
}
