using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ordering.Api.Consumer;
using Ordering.Api.Data;
using Ordering.Api.Options;
using Ordering.Api.Services;
using static MassTransit.Logging.OperationName;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDbContextConnection")));

builder.Services.AddScoped<IOrderService, OrderService>();

// MassTransit
builder.Services.AddMassTransit(e =>
{
    e.AddConsumer<OrderConsumer>();
    e.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

        configurator.Host(rabbitMQSettings!.Host, h => { h.Username("user"); h.Password("user123"); });
        configurator.ConfigureEndpoints(context);
    });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

    // Ensure that always create the new database if if is not exists

    logger.LogInformation("==>> Start Running migrations with connection string \n" + builder.Configuration.GetConnectionString("OrderingDbContextConnection"));
    await context.Database.MigrateAsync();
    logger.LogInformation("==>> End Running migrations");
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
