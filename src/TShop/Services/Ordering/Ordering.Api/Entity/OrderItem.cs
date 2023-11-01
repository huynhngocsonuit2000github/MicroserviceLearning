using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ordering.Api.Entity
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int OrderId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}
