using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Users.Api.Entity;
using Users.Api.Options;

namespace Users.Api.Data
{
    public class UserContext : IUserContext
    {
        public UserContext(IOptions<UserDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            Users = database.GetCollection<User>(settingValue.UsersCollectionName);
        }
        public IMongoCollection<User> Users { get; }
    }
}
