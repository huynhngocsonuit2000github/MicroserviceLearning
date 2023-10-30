namespace Discounts.Api.Entity
{
    public class Discount
    {
        public string Id { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
