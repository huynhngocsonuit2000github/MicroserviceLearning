using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Authenticate.Api.Entity
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
