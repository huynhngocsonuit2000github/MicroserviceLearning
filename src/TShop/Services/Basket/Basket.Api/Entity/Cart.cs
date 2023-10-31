using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Basket.Api.Entity
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public List<string> CartItemIds { get; set; } = null!;
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
