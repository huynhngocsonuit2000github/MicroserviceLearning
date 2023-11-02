using Basket.Api.Entity;
using Basket.Api.Model;
using Basket.Api.Repository;
using Basket.Api.SyncData;
using Common;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
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
        private readonly IDiscountproGrpc _discountproGrpc;
        private readonly IProductproGrpc _productproGrpc;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketsController(ICartRepository repository, ILogger<BasketsController> logger, ICartItemRepository cartItemRepository, IDiscountproGrpc discountproGrpc, IProductproGrpc productproGrpc, IPublishEndpoint publishEndpoint)
        {
            _cartRepository = repository;
            _logger = logger;
            _cartItemRepository = cartItemRepository;
            _discountproGrpc = discountproGrpc;
            _productproGrpc = productproGrpc;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CartResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CartResponse>> GetAllCarts()
        {
            _logger.LogInformation("==>> Start GetAllCarts");
            var carts = await _cartRepository.GetCarts();

            if (carts is null)
                return Ok(new List<CartResponse>());

            var res = new List<CartResponse>();

            foreach (var cart in carts)
            {
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

                res.Add(response);
            }

            return Ok(res);
        }

        [HttpGet("{id:length(24)}", Name = "GetCartById")]
        [ProducesResponseType(typeof(CartResponse), (int)HttpStatusCode.OK)]
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
        [ProducesResponseType(typeof(CartResponse), (int)HttpStatusCode.OK)]
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
            var productDiscountPro = await _discountproGrpc.GetDiscountByProductIdAsync(request.ProductId);
            var productPro = await _productproGrpc.GetByIdAsync(request.ProductId);
            //var product = ///

            var existingCartItem = await _cartItemRepository.GetCartItemByProductId(request.ProductId);
            if (existingCartItem is null)
            {
                var newCartItem = new CartItem()
                {
                    CartId = currentCart.Id.ToString(),
                    OriginalPrice = (decimal)productPro.Price, // from frpc Catalog
                    FinalPrice = (decimal)productPro.Price - (decimal)productDiscountPro.Amount, // from grpc, Catalog and Discount
                    ProductId = request.ProductId,
                    ProductName = productPro.Name, // from grpc Catalog
                    Quantity = request.Quantity,
                };
                await _cartItemRepository.CreateCartItem(newCartItem);
                currentCart.CartItemIds.Add(newCartItem.Id);
            }
            else
            {
                existingCartItem.OriginalPrice = (decimal)productPro.Price;// from frpc Catalog
                existingCartItem.FinalPrice = (decimal)productPro.Price - (decimal)productDiscountPro.Amount;// from grpc, Catalog and Discount
                existingCartItem.Quantity = request.Quantity;
                await _cartItemRepository.UpdateCartItem(existingCartItem);
            }

            var allCartItems = await _cartItemRepository.GetCartItemByCartId(currentCart.Id.ToString()); ;

            currentCart.OriginalPrice = allCartItems.Sum(e => e.OriginalPrice * e.Quantity);
            currentCart.FinalPrice = allCartItems.Sum(e => e.FinalPrice * e.Quantity);
            var res = await _cartRepository.UpdateCart(currentCart);

            return NoContent();
        }

        [HttpPost("Checkout/{userId}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Checkout(string userId)
        {
            _logger.LogInformation("==>> Start Checkout and send message to the message queue");

            var cart = await _cartRepository.GetCartByUserId(userId);

            if (cart is null)
            {
                _logger.LogWarning("==>> The cart corresponding with this user is not found");
                return NotFound();
            }

            var cartItems = await _cartItemRepository.GetCartItemByCartId(cart.Id.ToString());

            if (cartItems is null || cartItems.Count() ==0)
            {
                _logger.LogWarning("==>> The is no item in the cart corresponding with this user is not found");
                return Ok("Ok");
            }

            await _publishEndpoint.Publish(new OrderRequest()
            {
                UserId = cart.UserId,
                OrderItems = cartItems.Select(e => new OrderItemRequest()
                {
                    OriginalPrice = e.OriginalPrice,
                    FinalPrice = e.FinalPrice,
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    Quantity = e.Quantity
                }).ToList()
            });

            // Chua remove, done remove
            await _cartRepository.DeleteCart(cart.Id);
            foreach (var item in cartItems)
            {
                await _cartItemRepository.DeleteCartItem(item.Id);
            }

            return Ok("Ok");
        }
    }
}
