using Discounts.Api.Entity;
using Discounts.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Discounts.Api.Data
{
    public class DiscountContext : IDiscountContext
    {
        public DiscountContext(IOptions<DiscountDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            Discounts = database.GetCollection<Discount>(settingValue.DiscountsCollectionName);
        }
        public IMongoCollection<Discount> Discounts { get; }
    }
}
