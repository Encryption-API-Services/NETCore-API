using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataLayer.Mongo.Entities
{
    public class LogRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Guid RequestId { get; set; }
        public bool IsStart { get; set; }
        public string Method { get; set; }
        public string Token { get; set; }
        public string IP { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
