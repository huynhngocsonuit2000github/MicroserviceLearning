using Grpc.Core;
using MongoDB.Bson;
using Users.Api.Model;
using Users.Api.Repository;

namespace Users.Api.Services
{
    public class UserproService : Userpro.UserproBase
    {
        private readonly ILogger<UserproService> _logger;
        private readonly IUserRepository _userRepository;
        public UserproService(ILogger<UserproService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public override async Task<LoginResponsepro> Login(LoginRequestpro request, ServerCallContext context)
        {
            _logger.LogInformation("==>> Start login: " + request.ToJson());

            var requestrepo = new LoginRequest()
            {
                Username = request.Username,
                Password = request.Password,
            };

            var result = await _userRepository.GetUserByLoginRequest(requestrepo);

            if(result is null)
            {
                var notFoundStatus = new Status(StatusCode.NotFound, "Resource not found");
                throw new RpcException(notFoundStatus);
            }

            var resultpro = new LoginResponsepro()
            {
                Address = result.Address,
                Email = result.Email,
                Id = result.Id,
                Name = result.Name,
                Phone = result.Phone,
                RoleId = result.RoleId,
            };

            return resultpro;
        }
    }
}
