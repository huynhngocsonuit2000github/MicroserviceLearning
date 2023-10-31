using Catalog.Api.Entity;
using Catalog.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<ProductDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            Products = database.GetCollection<Product>(settingValue.ProductsCollectionName);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
