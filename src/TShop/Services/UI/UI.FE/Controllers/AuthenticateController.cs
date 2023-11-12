using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UI.FE.Data;
using UI.FE.Models;
using System.Security.Principal;

namespace UI.FE.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly ILogger<AuthenticateController> _logger;
        private readonly IAuthenticateHttpClient _authenticateHttpClient;

        public AuthenticateController(ILogger<AuthenticateController> logger, IAuthenticateHttpClient authenticateHttpClient)
        {
            _logger = logger;
            _authenticateHttpClient = authenticateHttpClient;
        }

        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var response = await _authenticateHttpClient.LoginAsync(model);

            if (response == null)
            {
                return View(model);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", response.UserId),
                new Claim("UserName", response.Username),
                new Claim("AccessToken", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjEyMzQ1Njc4OTAxMjM0NTY3ODkwMDAwMiIsIk5hbWUiOiJNZW1iZXIiLCJSb2xlSWQiOiIxMjM0NTY3ODkwMTIzNDU2Nzg5MDAwMDIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJNZW1iZXIiLCJleHAiOjE2OTk2NDI4MjIsImlzcyI6ImxhbmFzbGRublNMREFOU0pOQTIyMzEyNDIzc2Fsa25mc2xuIiwiYXVkIjoibGFuYXNsZG5uU0xEQU5TSk5BMjIzMTI0MjNzYWxrbmZzbG4ifQ.cF4KKTVsD8XRUcGiN6cYlnDOtAmahl06nHI4btcWUhE") // Add a custom claim for the token
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(1),
                IsPersistent = true,
            };

            // Sign in the user with the claims (including the custom token claim)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


            // Redirect to another action or return a success message
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to another action or return a success message
            return RedirectToAction("Login");
        }
    }
}
