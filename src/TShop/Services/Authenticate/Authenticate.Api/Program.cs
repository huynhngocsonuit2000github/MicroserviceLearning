using Authenticate.Api.Data;
using Authenticate.Api.Options;
using Authenticate.Api.Repository;
using Authenticate.Api.SyncData;
using Authenticate.Api.Utils;
using Catalog.Api.Data;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.Configure<AuthenticateDatabaseSettings>(builder.Configuration.GetSection("AuthenticateDatabaseSettings"));

builder.Services.AddScoped<IAuthenticateContext, AuthenticateContext>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
// Need to append the options to disable check https certificate
builder.Services.AddScoped(e =>
{
    var address = builder.Configuration.GetSection("GrpcService:User:UserApiUrl")!.Value!.ToString()!;
    var options = new GrpcChannelOptions()
    {
        HttpHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }
    };

    var channel = GrpcChannel.ForAddress(address, options);
    return channel;
});

builder.Services.AddScoped<IUserproGrpc, UserproGrpc>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});




// Jwt service
builder.Services.AddScoped<IJwtUtils, JwtUtils>();

// Jwt config for authenticate
string issuer = builder.Configuration["Tokens:Issuer"]!.ToString();
string signingKey = builder.Configuration["Tokens:Key"]!.ToString();
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<IAuthenticateContext>();
    var logger = services.GetRequiredService<ILogger<IAuthenticateContext>>();

    // Ensure that always create the new database if if is not exists 
    await SeedingData.Seeding(context, logger);
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});



// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
