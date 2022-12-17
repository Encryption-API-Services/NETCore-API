﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataLayer.Mongo.Entities
{
    public class EASException
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ExceptionBody { get; set; }
        public string Method { get; set; }
        public DateTime CreateDate { get; set; }
    }
}