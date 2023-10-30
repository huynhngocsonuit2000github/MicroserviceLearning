using Authenticate.Api.Entity;

namespace Authenticate.Api.Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRole(string id);
        Task CreateRole(Role role);
        Task<bool> UpdateRole(Role role);
        Task<bool> DeleteRole(string id);
        Task<Role> GetRoleByName(string name);
    }
}
