using DataLayer.Mongo.Entities;
using Encryption;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<SuccessfulLogin>> GetAllSuccessfulLoginWithinTimeFrame(string userId, DateTime dateTime)
        {
            return await this._successfulLoginRepository.FindAsync(x => x.UserId == userId &&
                                                                        x.CreateTime >= dateTime &&
                                                                        x.WasThisMe == false &&
                                                                        x.HasBeenChecked == false).GetAwaiter().GetResult().ToListAsync();
        }

        public async Task InsertSuccessfulLogin(SuccessfulLogin login)
        {
            await this._successfulLoginRepository.InsertOneAsync(login);
        }

        public async Task UpdateSuccessfulLoginWasMe(string loginId, bool wasThisMe)
        {
            var filter = Builders<SuccessfulLogin>.Filter.Eq(x => x.Id, loginId);
            var update = Builders<SuccessfulLogin>.Update.Set(x => x.WasThisMe, wasThisMe)
                                                         .Set(x => x.HasBeenChecked, true);
            await this._successfulLoginRepository.UpdateOneAsync(filter, update);
        }
    }
}
