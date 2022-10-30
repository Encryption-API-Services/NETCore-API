using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        public bool IsLockedOut { get; set; }
        public EmailActivationToken EmailActivationToken { get; set; }
        public ForgotPassword ForgotPassword { get; set; }

        public JwtToken JwtToken { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }

    public class EmailActivationToken
    {
        public string Token { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public byte[] SignedToken { get; set; }
    }

    public class JwtToken
    {
        public string Token { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }

    public class ForgotPassword
    {
        public string Token { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public byte[] SignedToken { get; set; }
        public bool HasBeenReset { get; set; }
    }
}