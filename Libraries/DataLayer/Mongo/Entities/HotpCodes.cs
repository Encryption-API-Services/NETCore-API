﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataLayer.Mongo.Entities
{
    public class HotpCodes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Hotp { get; set; }
        public long Counter { get; set; }
    }
}
