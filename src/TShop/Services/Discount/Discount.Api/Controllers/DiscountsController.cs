using Discounts.Api.Entity;
using Discounts.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Discounts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountsController> _logger;
        public DiscountsController(IDiscountRepository repository, ILogger<DiscountsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
         
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Discount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Discount>>> GetDiscounts()
        {
            _logger.LogInformation("==>> Start GetDiscounts");
            var discounts = await _repository.GetDiscounts();
            return Ok(discounts); 
        }

        [HttpGet("{id:length(24)}", Name = "GetDiscount")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Discount>> GetDiscountById(string id)
        {
            _logger.LogInformation($"==>> Start GetDiscountById: {id}");
            var discount = await _repository.GetDiscount(id);
            if (discount == null)
            {
                _logger.LogError($"==>> Discount with id: {id}, not found.");
                return NotFound();
            }
            return Ok(discount);
        }

        [Route("[action]/{productId}", Name = "GetDiscountByProductId")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Discount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Discount>>> GetDiscountByProductId(string productId)
        {
            _logger.LogInformation($"==>> Start GetDiscountByProductId: {productId}");
            var discounts = await _repository.GetDiscountByProductId(productId);
            return Ok(discounts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Discount>> CreateDiscount([FromBody] Discount discount)
        {
            _logger.LogInformation("==>> Start CreateDiscount: \n" + discount.ToJson()); 
            await _repository.CreateDiscount(discount);
            return CreatedAtRoute("GetDiscount", new { id = discount.Id }, discount);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Discount discount)
        {
            _logger.LogInformation("==>> Start UpdateDiscount: \n" + discount.ToJson());
            return Ok(await _repository.UpdateDiscount(discount));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscountById(string id)
        {
            _logger.LogInformation("==>> Start DeleteDiscountById: " + id);
            return Ok(await _repository.DeleteDiscount(id));
        }
    }
}
