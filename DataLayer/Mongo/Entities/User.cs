using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NSec.Cryptography;
using System;

namespace DataLayer.Mongo.Entities
{
    
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public Key ED25519 { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
