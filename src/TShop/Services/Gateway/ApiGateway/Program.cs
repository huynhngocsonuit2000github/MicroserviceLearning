using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"ocelot.{builder.Environment.EnvironmentName}.json");

// Add services to the container.
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();
builder.Services.AddOcelot(builder.Configuration);


//builder.Services.AddControllers();

var app = builder.Build();

await app.UseOcelot(); 

// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

app.Run();
