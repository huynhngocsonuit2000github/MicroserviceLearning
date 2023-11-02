using Common;
using MassTransit;
using Newtonsoft.Json;

namespace Ordering.Api.Consumer
{
    public class OrderConsumer : IConsumer<OrderRequest>
    {
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderRequest> context)
        {
            _logger.LogInformation("==>> Start Consum from Basket: " + JsonConvert.SerializeObject(context.Message));
            OrderRequest message = context.Message;

            return Task.CompletedTask;
        }
    }
}
