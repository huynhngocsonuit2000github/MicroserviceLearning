using Authenticate.Api.Entity;
using Authenticate.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Authenticate.Api.Data
{
    public class AuthenticateContext : IAuthenticateContext
    {
        public AuthenticateContext(IOptions<AuthenticateDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            Roles = database.GetCollection<Role>(settingValue.RolesCollectionName);
        }
        public IMongoCollection<Role> Roles { get; }
    }
}
