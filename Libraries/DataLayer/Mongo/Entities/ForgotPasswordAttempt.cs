using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Mongo.Entities
{
    public class ForgotPasswordAttempt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
