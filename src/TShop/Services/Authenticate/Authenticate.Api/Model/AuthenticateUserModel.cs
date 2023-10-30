namespace Authenticate.Api.Model
{
    public class AuthenticateUserModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}
