using Catalog.Api.Controllers;
using Catalog.Api.Entity;
using Microsoft.AspNetCore.Routing.Tree;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public static class SeedingData
    {
        public static async Task Seeding(ICatalogContext context, ILogger<ICatalogContext> logger)
        {
            if (await context.Products.CountAsync(e => true) != 0) return;

            logger.LogInformation("==>> Start seeding data");
            await context.Products.InsertManyAsync(new List<Product>()
                {
                    new Product()
                    {
                        Id = "123456789012345678900001",
                        CategoryName = "Phone",
                        Description = "IPhone 11 description",
                        Name = "IPhone 11 name",
                        Price = 11,
                        Summary = "IPhone 11 summary",
                        ImageFile = "IPhone 11 image"
                    },
                    new Product()
                    {
                        Id = "123456789012345678900002",
                        CategoryName = "Phone",
                        Description = "IPhone 12 description",
                        Name = "IPhone 12 name",
                        Price = 12,
                        Summary = "IPhone 12 summary",
                        ImageFile = "IPhone 12 image"
                    },
                    new Product()
                    {
                        Id = "123456789012345678900003",
                        CategoryName = "Phone",
                        Description = "IPhone 13 description",
                        Name = "IPhone 13 name",
                        Price = 13,
                        Summary = "IPhone 13 summary",
                        ImageFile = "IPhone 13 image"
                    },
                    new Product()
                    {
                        Id = "123456789012345678900004",
                        CategoryName = "Phone",
                        Description = "IPhone 14 description",
                        Name = "IPhone 14 name",
                        Price = 14,
                        Summary = "IPhone 14 summary",
                        ImageFile = "IPhone 14 image"
                    },
                    new Product()
                    {
                        Id = "123456789012345678900005",
                        CategoryName = "Laptop",
                        Description = "Macbook Pro 2018 description",
                        Name = "Macbook Pro 2018 name",
                        Price = 20,
                        Summary = "Macbook Pro 2018 summary",
                        ImageFile = "Macbook Pro 2018 image"
                    },
                });

            logger.LogInformation("==>> End seeding data");
        }
    }
}
