namespace UI.FE.Models
{
    public class CartResponse
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public List<CartItemResponse> CartItemIds { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
