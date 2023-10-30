using Authenticate.Api.Data;
using Authenticate.Api.Entity;
using Authenticate.Api.Repository;
using MongoDB.Driver;

namespace Authenticate.Api.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IAuthenticateContext _context;
        public RoleRepository(IAuthenticateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _context
                            .Roles
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<Role> GetRole(string id)
        {
            return await _context
                           .Roles
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }
        public async Task<Role> GetRoleByName(string name)
        {
            return await _context
                           .Roles
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }
        public async Task CreateRole(Role role)
        {
            await _context.Roles.InsertOneAsync(role);
        }
        public async Task<bool> UpdateRole(Role role)
        {
            var updateResult = await _context
                                        .Roles
                                        .ReplaceOneAsync(filter: g => g.Id == role.Id, replacement: role);
            return updateResult.IsAcknowledged
                                && updateResult.ModifiedCount > 0;
        }
        public async Task<bool> DeleteRole(string id)
        {
            FilterDefinition<Role> filter = Builders<Role>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                                                            .Roles
                                                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                            && deleteResult.DeletedCount > 0;
        }
    }
}
