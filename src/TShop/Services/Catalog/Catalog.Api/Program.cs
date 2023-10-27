using Catalog.Api.Data;
using Catalog.Api.Entity;
using Catalog.Api.Options;
using Catalog.Api.Repository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabaseSettings"));
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ICatalogContext>();
    var logger = services.GetRequiredService<ILogger<ICatalogContext>>();

    // Ensure that always create the new database if if is not exists
    // await context.Database.EnsureCreatedAsync();
    await SeedingData.Seeding(context, logger);
}


app.UseAuthorization();

app.MapControllers();

app.Run();
