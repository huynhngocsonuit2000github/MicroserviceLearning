using Users.Api.Entity;
using Users.Api.Model;

namespace Users.Api.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string id);
        Task CreateUser(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string id);
        Task<User> GetUserByLoginRequest(LoginRequest request);
    }
}
