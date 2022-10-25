using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Common;

namespace DataLayer.Mongo.Entities
{
    public class BenchmarkMethod
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public BenchmarkMethodLogger Details { get; set; }
    }
}
