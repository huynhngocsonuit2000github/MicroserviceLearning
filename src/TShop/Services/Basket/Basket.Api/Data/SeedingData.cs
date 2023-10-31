using Basket.Api.Data;
using Basket.Api.Entity;
using MongoDB.Driver;

namespace Basket.Api.Data
{
    public static class SeedingData
    {
        public static async Task Seeding(IBasketContext context, ILogger<IBasketContext> logger)
        {
            if (await context.CartItems.CountAsync(e => true) != 0 ||
                await context.Carts.CountAsync(e => true) != 0) return;

            logger.LogInformation("==>> Start seeding data");
            await context.Carts.InsertManyAsync(new List<Cart>()

                {
                    new Cart()
                    {
                        Id = "123456789012345678900001",
                        UserId = "123456789012345678900001",
                        CartItemIds = new List<string>()
                        {
                            "123456789012345678900001"
                        },
                        OriginalPrice = 22,
                        FinalPrice = 20,
                    }
                });

            await context.CartItems.InsertManyAsync(new List<CartItem>()
            {
                new CartItem()
                {
                    Id = "123456789012345678900001",
                    Quantity = 2,
                    ProductId = "123456789012345678900001",
                    CartId = "123456789012345678900001",
                    ProductName = "Phone",
                    OriginalPrice = 11,
                    FinalPrice = 10,
                }
            });

            logger.LogInformation("==>> End seeding data");
        }
    }
}
