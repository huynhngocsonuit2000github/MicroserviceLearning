using Authenticate.Api.Entity;
using MongoDB.Driver;

namespace Authenticate.Api.Data
{
    public interface IAuthenticateContext
    {
        IMongoCollection<Role> Roles { get; }
    }
}