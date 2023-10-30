using Authenticate.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate.Api.SyncData
{
    public interface IUserproGrpc
    {
        Task<LoginResponse?> Login([FromBody] LoginRequest request);
    }
}