using Microsoft.AspNetCore.Authentication.Cookies;
using UI.FE.Data;
using UI.FE.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(nameof(AppSettings)));

builder.Services.AddScoped<IProductHttpClient, ProductHttpClient>();
builder.Services.AddScoped<IAuthenticateHttpClient, AuthenticateHttpClient>();
builder.Services.AddScoped<IBasketHttpClient, BasketHttpClient>();
builder.Services.AddScoped(e =>
{
    var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
    HttpClientHandler clientHandler = new HttpClientHandler();
    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

    return new HttpClient(clientHandler)
    {
        BaseAddress = new Uri(appSettings!.BaseServerUrl)
    };
});

// Authenticate
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Authenticate/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
