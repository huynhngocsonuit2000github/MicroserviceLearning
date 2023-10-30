# Install swagger if any
Install package
    dotnet add package Swashbuckle.AspNetCore (if any)


services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

# Install MongoDb for Dotnet Core
Pull image
	docker pull mongo

Run docker image
	docker run -d -p 27017:27017 --name tshop-mongo mongo

Install essential nuget packages
	dotnet add package MongoDB.Driver

Create Product class
Create connection string information inside appsettin g										
   "ProductDatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "product",
    "BooksCollectionName": "products"
  }

Create class option setting
	ProductDatabaseSettings {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;
    }

Registe inside appsetting
    builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabaseSettings"));


Create dbcontext class
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IOptions<ProductDatabaseSettings> settings)
        {
            var settingValue = settings.Value;

            var client = new MongoClient(settingValue.ConnectionString);
            var database = client.GetDatabase(settingValue.DatabaseName);
            Products = database.GetCollection<Product>(settingValue.ProductsCollectionName);
            //CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }


Register dbcontext
    builder.Services.AddScoped<ICatalogContext, CatalogContext>();


Create product repository and register it inside the program file
    IProductRepository and ProductRepository

    
https://www.dotnetcoban.com/2019/09/mongodb-in-asp-dotnet-core.html
https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-7.0&tabs=visual-studio