using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ordering.Api.Entity
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<OrderItem> OrderItems { get; set; } = null!;
    }
}
