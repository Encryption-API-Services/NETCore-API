using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

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
        public async Task<long> GetHighestCounter()
        {
            HotpCode hotpCode = await this._hotpCodes.AsQueryable().OrderByDescending(x => x.Counter).FirstAsync();
            return hotpCode.Counter;
        }
        public async Task InsertHotpCode(HotpCode code)
        {
            await this._hotpCodes.InsertOneAsync(code);
        }
    }
}
