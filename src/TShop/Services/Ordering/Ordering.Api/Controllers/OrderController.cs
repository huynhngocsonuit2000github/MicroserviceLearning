using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Model;
using Ordering.Api.Services;

namespace Ordering.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] OrderRequest request)
        {
            _logger.LogInformation("==>> Start Checkout: " + request);
            var result = await _orderService.Create(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("==>> Start GetAll");
            var result = await _orderService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("==>> Start GetById");
            var result = await _orderService.GetById(id);
            return Ok(result);
        }
    }
}
