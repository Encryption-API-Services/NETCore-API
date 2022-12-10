using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace DataLayer.Mongo.Entities
{
    public class BlogPost
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string BlogTitle { get; set; }
        public string BlogBody { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
