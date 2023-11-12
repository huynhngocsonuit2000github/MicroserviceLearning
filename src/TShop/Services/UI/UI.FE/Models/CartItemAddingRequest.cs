namespace UI.FE.Models
{
    public class CartItemAddingRequest
    {
        public string UserId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int? Quantity { get; set; } = null!;
    }
}
