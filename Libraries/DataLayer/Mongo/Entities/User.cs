using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Runtime;

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
        public bool IsAdmin { get; set; }
        public string StripCustomerId { get; set; }
        public string StripCardId { get; set; }
        public string ApiKey { get; set; }
        public Phone2FA Phone2FA { get; set; }
        public LockedOut LockedOut { get; set; }
        public EmailActivationToken EmailActivationToken { get; set; }
        public ForgotPassword ForgotPassword { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
    public class Phone2FA
    {
        public string PhoneNumber { get; set; }
        public bool IsEnabled { get; set; }
    }
    public class EmailActivationToken
    {
        public string Token { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public byte[] SignedToken { get; set; }
        public bool WasVerified { get; set; }
        public bool WasSent { get; set; }
    }

    public class ForgotPassword
    {
        public string Token { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public byte[] SignedToken { get; set; }
        public bool HasBeenReset { get; set; }
    }

    public class LockedOut
    {
        public bool IsLockedOut { get; set; }
        public bool HasBeenSentOut { get; set; }
    }
}