namespace Discounts.Api.Options
{
    public class DiscountDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string DiscountsCollectionName { get; set; } = null!;
    }
}
