namespace Authenticate.Api.Options
{
    public class AuthenticateDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string RolesCollectionName { get; set; } = null!;
    }
}
