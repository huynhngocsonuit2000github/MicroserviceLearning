using MongoDB.Driver;
using Users.Api.Entity;

namespace Users.Api.Data
{
    public interface IUserContext
    {
        IMongoCollection<User> Users { get; }
    }
}