using Basket.Api.Data;
using Basket.Api.Options;
using Basket.Api.Repository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<BasketDatabaseSettings>(
    builder.Configuration.GetSection("BasketDatabaseSettings"));

builder.Services.AddScoped<IBasketContext, BasketContext>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<IBasketContext>();
    var logger = services.GetRequiredService<ILogger<IBasketContext>>();

    // Ensure that always create the new database if if is not exists
    // await context.Database.EnsureCreatedAsync();
    await SeedingData.Seeding(context, logger);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
