namespace Common
{
    public class OrderRequest
    {
        public string UserId { get; set; } = null!;
        public List<OrderItemRequest> OrderItems { get; set; } = null!;
    }
}