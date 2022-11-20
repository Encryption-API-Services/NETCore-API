﻿using DataLayer.Mongo.Entities;
using Encryption;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Mongo.Repositories
{
    public class SuccessfulLoginRepository : ISuccessfulLoginRepository
    {
        private readonly IMongoCollection<SuccessfulLogin> _successfulLoginRepository;
        public SuccessfulLoginRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._successfulLoginRepository = database.GetCollection<SuccessfulLogin>("SuccessfulLogins");
        }
    }
}
