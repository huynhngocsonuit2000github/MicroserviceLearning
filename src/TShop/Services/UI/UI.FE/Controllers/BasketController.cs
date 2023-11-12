using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.FE.Data;
using UI.FE.Models;

namespace UI.FE.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketHttpClient _basketHttpClient;

        public BasketController(ILogger<BasketController> logger,
            IBasketHttpClient basketHttpClient)
        {
            _logger = logger;
            _basketHttpClient = basketHttpClient;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims?.FirstOrDefault(e => e.Type == "UserId")?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            var cart = await _basketHttpClient.GetCartByUserIdAsync(userId.ToString());
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(CartItemAddingRequest request)
        {
            await _basketHttpClient.AddToCart(request);

            return RedirectToAction("Index");
        }
    }
}
