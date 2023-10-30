using Authenticate.Api.Model;
using Authenticate.Api.Repository;
using Authenticate.Api.Utils;
using Grpc.Net.Client;
using MongoDB.Bson;

namespace Authenticate.Api.SyncData
{
    public class UserproGrpc : IUserproGrpc
    {
        private readonly GrpcChannel _channel;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<UserproGrpc> _logger;
        private readonly IJwtUtils _jwtUtils;

        public UserproGrpc(GrpcChannel channel, IRoleRepository roleRepository, ILogger<UserproGrpc> logger, IJwtUtils jwtUtils)
        {
            _channel = channel;
            _roleRepository = roleRepository;
            _logger = logger;
            _jwtUtils = jwtUtils;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var client = new Userpro.UserproClient(_channel);

            var requestori = new LoginRequestpro()
            {
                Username = request.Username,
                Password = request.Password,
            };

            LoginResponsepro? reply = null;

            try
            {
                _logger.LogInformation("==>> Start calling Grpc to ask for login");
                reply = await client.LoginAsync(requestori);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError("Login failed with " + request.ToJson());

                return null!;
            }

            // Create Token-Jwt
            var role = await _roleRepository.GetRole(reply.RoleId);

            var token = _jwtUtils.GenerateToken(new AuthenticateUserModel()
            {
                Id = reply.Id,
                Name = reply.Name,
                RoleId = reply.RoleId,
                RoleName = role.Name
            });
             
            var response = new LoginResponse()
            {
                Token = token
            };

            return response;
        }


    }
}
