using Users.Api.Data;
using Users.Api.Entity;
using MongoDB.Driver;

namespace Users.Api.Data
{
    public static class SeedingData
    {
        public static async Task Seeding(IUserContext context, ILogger<IUserContext> logger)
        {
            if (await context.Users.CountAsync(e => true) != 0) return;

            logger.LogInformation("==>> Start seeding data");
            await context.Users.InsertManyAsync(new List<User>()
                {
                    new User()
                    {
                        Id = "123456789012345678900001",
                        Name = "Admin",
                        Username = "admin",
                        Password = "admin",
                        RoleId = "123456789012345678900001",
                        RoleName = "Admin",
                        Address = "HCM",
                        Email = "admin@gmail.com",
                        Phone = "0123456789"
                    },
                    new User()
                    {
                        Id = "123456789012345678900002",
                        Name = "Member",
                        Username = "member",
                        Password = "member",
                        RoleId = "123456789012345678900002",
                        RoleName = "Member",
                        Address = "Ha Noi",
                        Email = "member@gmail.com",
                        Phone = "0987654321"
                    },
                });

            logger.LogInformation("==>> End seeding data");
        }
    }
}
