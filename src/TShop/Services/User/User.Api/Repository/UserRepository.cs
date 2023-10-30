using MongoDB.Driver;
using Users.Api.Data;
using Users.Api.Entity;
using Users.Api.Model;

namespace Users.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserContext _context;
        public UserRepository(IUserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context
                            .Users
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<User> GetUser(string id)
        {
            return await _context
                           .Users
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }
        public async Task CreateUser(User user)
        {
            await _context.Users.InsertOneAsync(user);
        }
        public async Task<bool> UpdateUser(User user)
        {
            var updateResult = await _context
                                        .Users
                                        .ReplaceOneAsync(filter: g => g.Id == user.Id, replacement: user);
            return updateResult.IsAcknowledged
                                && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteUser(string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                            .Users
                                                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                            && deleteResult.DeletedCount > 0;
        }
        public async Task<User> GetUserByLoginRequest(LoginRequest request)
        {
            return await _context
                           .Users
                           .Find(p => p.Username == request.Username && p.Password == request.Password)
                           .FirstOrDefaultAsync();
        }
    }
}
