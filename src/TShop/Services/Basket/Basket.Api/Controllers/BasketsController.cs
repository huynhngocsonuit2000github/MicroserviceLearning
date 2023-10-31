using Basket.Api.Entity;
using Basket.Api.Model;
using Basket.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<BasketsController> _logger;
        private readonly ICartItemRepository _cartItemRepository;

        public BasketsController(ICartRepository repository, ILogger<BasketsController> logger, ICartItemRepository cartItemRepository)
        {
            _cartRepository = repository;
            _logger = logger;
            _cartItemRepository = cartItemRepository;
        }

        [HttpGet("{id:length(24)}", Name = "GetCartById")]
        [ProducesResponseType(typeof(IEnumerable<Cart>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CartResponse>> GetCartById(string id)
        {
            _logger.LogInformation("==>> Start GetCartById: " + id);
            var cart = await _cartRepository.GetCart(id);

            if (cart is null)
                return Ok(new List<Cart>());

            var cartItems = await _cartItemRepository.GetCartItemByCartId(cart.Id.ToString());

            var response = new CartResponse()
            {
                Id = cart.Id.ToString(),
                FinalPrice = cart.FinalPrice,
                OriginalPrice = cart.OriginalPrice,
                UserId = cart.UserId,
                CartItemIds = cartItems.Select(e => new CartItemResponse()
                {
                    Id = e.Id,
                    OriginalPrice = e.OriginalPrice,
                    FinalPrice = e.FinalPrice,
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    Quantity = e.Quantity
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("GetCartByUserId/{userId}", Name = "GetCartByUserId")]
        [ProducesResponseType(typeof(IEnumerable<Cart>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CartResponse>> GetCartByUserId(string userId)
        {
            _logger.LogInformation("==>> Start GetCartByUserId: " + userId);
            var cart = await _cartRepository.GetCartByUserId(userId);

            if (cart is null)
                return Ok(new List<Cart>());

            var cartItems = await _cartItemRepository.GetCartItemByCartId(cart.Id.ToString());

            var response = new CartResponse()
            {
                Id = cart.Id.ToString(),
                FinalPrice = cart.FinalPrice,
                OriginalPrice = cart.OriginalPrice,
                UserId = cart.UserId,
                CartItemIds = cartItems.Select(e => new CartItemResponse()
                {
                    Id = e.Id,
                    OriginalPrice = e.OriginalPrice,
                    FinalPrice = e.FinalPrice,
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    Quantity = e.Quantity
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPost("AddToCart")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddToCart([FromBody] CartItemAddingRequest request)
        {
            _logger.LogInformation("==>> Start AddCartItemToCart: \n" + request.ToJson());

            var existingCart = await _cartRepository.GetCartByUserId(request.UserId);
            if (existingCart is null)
            {
                await _cartRepository.CreateCart(new Cart()
                {
                    UserId = request.UserId,
                    OriginalPrice = 0,
                    FinalPrice = 0,
                    CartItemIds = new List<string>()
                });
            }

            // Reget
            var currentCart = await _cartRepository.GetCartByUserId(request.UserId);

            // Call Grpc to get product information
            //var product = ///

            var existingCartItem = await _cartItemRepository.GetCartItemByProductId(request.ProductId);
            if (existingCartItem is null)
            {
                var newCartItem = new CartItem()
                {
                    CartId = currentCart.Id.ToString(),
                    OriginalPrice = 5, // from frpc Catalog
                    FinalPrice = 5, // from grpc, Catalog and Discount
                    ProductId = request.ProductId,
                    ProductName = "", // from grpc Catalog
                    Quantity = request.Quantity,
                };
                await _cartItemRepository.CreateCartItem(newCartItem);
                currentCart.CartItemIds.Add(newCartItem.Id);
            }
            else
            {
                existingCartItem.OriginalPrice = 5;// from frpc Catalog
                existingCartItem.FinalPrice = 5;// from grpc, Catalog and Discount
                existingCartItem.Quantity = request.Quantity;
                await _cartItemRepository.UpdateCartItem(existingCartItem);
            }

            var allCartItems = await _cartItemRepository.GetCartItemByCartId(currentCart.Id.ToString()); ;

            currentCart.OriginalPrice = allCartItems.Sum(e => e.OriginalPrice * e.Quantity);
            currentCart.FinalPrice = allCartItems.Sum(e => e.FinalPrice * e.Quantity);
            var res = await _cartRepository.UpdateCart(currentCart);

            return NoContent();
        }
    }
}
