﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataLayer.Mongo.Entities
{
    public class Newsletter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
