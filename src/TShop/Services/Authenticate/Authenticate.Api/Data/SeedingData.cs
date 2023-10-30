using Authenticate.Api.Data;
using Authenticate.Api.Entity;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public static class SeedingData
    {
        public static async Task Seeding(IAuthenticateContext context, ILogger<IAuthenticateContext> logger)
        {
            if (await context.Roles.CountAsync(e => true) != 0) return;

            logger.LogInformation("==>> Start seeding data");
            await context.Roles.InsertManyAsync(new List<Role>()
                {
                    new Role()
                    {
                        Id = "123456789012345678900001",
                        Description = "Admin",
                        Name = "Admin",
                    },
                    new Role()
                    {
                        Id = "123456789012345678900002",
                        Description = "Member",
                        Name = "Member",
                    },
                });

            logger.LogInformation("==>> End seeding data");
        }
    }
}
