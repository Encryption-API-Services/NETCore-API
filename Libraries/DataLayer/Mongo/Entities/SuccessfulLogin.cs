﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataLayer.Mongo.Entities
{
    public class SuccessfulLogin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
    }
}
