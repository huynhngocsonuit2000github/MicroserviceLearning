using UI.FE.Models;

namespace UI.FE.Data
{
    public interface IAuthenticateHttpClient
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}