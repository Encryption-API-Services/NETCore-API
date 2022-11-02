using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataLayer.Mongo.Entities
{
    public class HashedPassword
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
