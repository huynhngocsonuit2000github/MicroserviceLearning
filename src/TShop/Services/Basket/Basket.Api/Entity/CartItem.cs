using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Basket.Api.Entity
{
    public class CartItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public int Quantity { get; set; }
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string CartId { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
