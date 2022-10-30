using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Mongo.Repositories
{
    public class FailedLoginAttemptRepository : IFailedLoginAttemptRepository
    {
        private readonly IMongoCollection<FailedLoginAttempt> _failedLoginAttempts;
        public FailedLoginAttemptRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._failedLoginAttempts = database.GetCollection<FailedLoginAttempt>("FailedLoginAttempts");
        }
    }
}
