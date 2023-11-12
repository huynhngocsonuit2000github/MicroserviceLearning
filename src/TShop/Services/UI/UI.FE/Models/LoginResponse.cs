namespace UI.FE.Models
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}
