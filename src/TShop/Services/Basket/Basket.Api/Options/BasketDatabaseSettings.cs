namespace Basket.Api.Options
{
    public class BasketDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string CartsCollectionName { get; set; } = null!;
        public string CartItemsCollectionName { get; set; } = null!;
    }
}
