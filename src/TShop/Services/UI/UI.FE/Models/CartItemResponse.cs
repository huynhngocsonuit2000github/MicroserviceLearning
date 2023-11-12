namespace UI.FE.Models
{
    public class CartItemResponse
    {
        public string Id { get; set; } = null!;
        public int Quantity { get; set; }
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
