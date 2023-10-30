using Authenticate.Api.Model;

namespace Authenticate.Api.Utils
{
    public interface IJwtUtils
    {
        string GenerateToken(AuthenticateUserModel model);
    }
}