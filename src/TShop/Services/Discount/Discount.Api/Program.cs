using Discounts.Api.Data;
using Discounts.Api.Options;
using Discounts.Api.Repository;
using Discounts.Api.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<DiscountDatabaseSettings>(
    builder.Configuration.GetSection("DiscountDatabaseSettings"));
builder.Services.AddScoped<IDiscountContext, DiscountContext>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddGrpc();

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

    var context = services.GetRequiredService<IDiscountContext>();
    var logger = services.GetRequiredService<ILogger<IDiscountContext>>();

    // Ensure that always create the new database if if is not exists
    // await context.Database.EnsureCreatedAsync();
    await SeedingData.Seeding(context, logger);
}


app.UseAuthorization();

app.MapGrpcService<DiscountproService>();
app.MapControllers();

app.Run();
