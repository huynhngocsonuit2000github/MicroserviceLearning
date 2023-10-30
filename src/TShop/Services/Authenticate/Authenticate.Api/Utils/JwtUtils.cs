using Authenticate.Api.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate.Api.Utils
{
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;

        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(AuthenticateUserModel model)
        {
            var claims = new List<Claim>()
            {
                    new Claim("Id", model.Id),
                    new Claim("Name", model.Name),
                    new Claim("RoleId", model.RoleId),
                    new Claim(ClaimTypes.Role, model.RoleName),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]!.ToString()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"]!.ToString(),
                _configuration["Tokens:Issuer"]!.ToString(),
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
