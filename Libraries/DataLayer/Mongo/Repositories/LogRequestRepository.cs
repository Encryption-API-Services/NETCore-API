using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class LogRequestRepository : ILogRequestRepository
    {
        private readonly IMongoCollection<LogRequest> _logRequestCollection;
        public LogRequestRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._logRequestCollection = database.GetCollection<LogRequest>("LogRequest");
        }

        public async Task<List<LogRequest>> GetTop10RequestsByIP(string ip)
        {
            return await this._logRequestCollection.AsQueryable().Where(x => x.IP == ip &&
                                                                        x.IsStart == false &&
                                                                        x.CreateTime >= DateTime.UtcNow.AddHours(-1)).ToListAsync();
        }

        public async Task InsertRequest(LogRequest request)
        {
            await this._logRequestCollection.InsertOneAsync(request);
        }
    }
}
