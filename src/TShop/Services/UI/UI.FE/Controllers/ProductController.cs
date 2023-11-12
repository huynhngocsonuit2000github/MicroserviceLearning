using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.FE.Data;

namespace UI.FE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductHttpClient _productHttpClient;
        private readonly IBasketHttpClient _basketHttpClient;

        public ProductController(ILogger<ProductController> logger,
            IProductHttpClient productHttpClient,
            IBasketHttpClient basketHttpClient)
        {
            _logger = logger;
            _productHttpClient = productHttpClient;
            _basketHttpClient = basketHttpClient;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var products = await _productHttpClient.GetAllProductsAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(string productId)
        {
            var userId = User.Claims.FirstOrDefault(e => e.Type == "UserId").Value;

            await _basketHttpClient.AddToCart(new Models.CartItemAddingRequest()
            {
                ProductId = productId,
                UserId = userId.ToString(),
                Quantity = null
            });

            return RedirectToAction("Index", "Basket");
        }
    }
}
