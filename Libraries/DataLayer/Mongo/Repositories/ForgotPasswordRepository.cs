using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly IMongoCollection<ForgotPasswordAttempt> _forgotPasswordAttempts;
        public ForgotPasswordRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._forgotPasswordAttempts = database.GetCollection<ForgotPasswordAttempt>("ForgotPasswordAttempts");
        }
        public Task<List<string>> GetLastFivePassword(string userId)
        {
            throw new NotImplementedException();
        }

        public Task InsertForgotPasswordAttempt(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
