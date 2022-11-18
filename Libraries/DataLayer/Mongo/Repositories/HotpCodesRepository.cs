using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Types;

namespace DataLayer.Mongo.Repositories
{
    public class HotpCodesRepository : IHotpCodesRepository
    {
        private readonly IMongoCollection<HotpCode> _hotpCodes;

        public HotpCodesRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._hotpCodes = database.GetCollection<HotpCode>("HotpCodes");
        }
        public async Task<List<HotpCode>> GetAllHotpCodesNotSent()
        {
            return await this._hotpCodes.AsQueryable().Where(x => x.HasBeenSent == false && x.HasBeenVerified == false).ToListAsync();
        }

        public async Task<long> GetHighestCounter()
        {
            HotpCode hotpCode = await this._hotpCodes.AsQueryable().OrderByDescending(x => x.Counter).FirstAsync();
            return hotpCode.Counter;
        }
        public async Task InsertHotpCode(HotpCode code)
        {
            await this._hotpCodes.InsertOneAsync(code);
        }
        public async Task UpdateHotpCodeToSent(string id)
        {
            var filter = Builders<HotpCode>.Filter.Eq(x => x.Id, id);
            var update = Builders<HotpCode>.Update.Set(x => x.HasBeenSent, true);
            await this._hotpCodes.UpdateOneAsync(filter, update);
        }
    }
}
