using Common;
using MassTransit;
using Newtonsoft.Json;
using Ordering.Api.Services;

namespace Ordering.Api.Consumer
{
    public class OrderConsumer : IConsumer<OrderRequest>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IOrderService _orderService;

        public OrderConsumer(ILogger<OrderConsumer> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<OrderRequest> context)
        {
            _logger.LogInformation("==>> Start Consum from Basket: " + JsonConvert.SerializeObject(context.Message));
            OrderRequest message = context.Message;

            await _orderService.Create(message);
             
            _logger.LogInformation("==>> End Consum from Basket");
        }
    }
}
