using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Mongo.Repositories
{
    public class HashedPasswordRepository : IHashedPasswordRepository
    {
        private readonly IMongoCollection<HashedPassword> _hashedPasswords;

        public HashedPasswordRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._hashedPasswords = database.GetCollection<HashedPassword>("HashedPasswords");
        }
    }
}
