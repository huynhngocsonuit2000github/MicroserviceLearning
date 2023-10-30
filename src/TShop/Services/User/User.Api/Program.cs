using Microsoft.OpenApi.Models;
using Users.Api.Data;
using Users.Api.Options;
using Users.Api.Repository;
using Users.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<UserDatabaseSettings>(builder.Configuration.GetSection("UserDatabaseSettings"));

builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});


builder.Services.AddGrpc();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<IUserContext>();
    var logger = services.GetRequiredService<ILogger<IUserContext>>();

    // Ensure that always create the new database if if is not exists 
    await SeedingData.Seeding(context, logger);
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseRouting();

app.UseAuthorization();

//app.MapGrpcService<GreeterService>();

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GreeterService>();
    endpoints.MapGrpcService<UserproService>();
});

// Map ASP.NET Core controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
