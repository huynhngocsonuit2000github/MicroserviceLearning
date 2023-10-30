using Discounts.Api.Entity;
using MongoDB.Driver;

namespace Discounts.Api.Data
{
    public static class SeedingData
    {
        public static async Task Seeding(IDiscountContext context, ILogger<IDiscountContext> logger)
        {
            if (await context.Discounts.CountAsync(e => true) != 0) return;

            logger.LogInformation("==>> Start seeding data");
            await context.Discounts.InsertManyAsync(new List<Discount>()
                {
                    new Discount()
                    {
                        Id = "123456789012345678900001",
                        ProductId = "123456789012345678900001",
                        ProductName = "Phone",
                        Description = "Discount for IPhone 11 description",
                        Amount = 5,
                    },
                    new Discount()
                    {
                        Id = "123456789012345678900002",
                        ProductId = "123456789012345678900002",
                        ProductName = "Phone",
                        Description = "Discount for IPhone 12 description",
                        Amount = 12,
                    },
                });

            logger.LogInformation("==>> End seeding data");
        }
    }
}
