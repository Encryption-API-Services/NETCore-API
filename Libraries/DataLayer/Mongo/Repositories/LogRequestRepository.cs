using DataLayer.Mongo.Entities;
using MongoDB.Driver;
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
        public async Task InsertRequest(LogRequest request)
        {
            await this._logRequestCollection.InsertOneAsync(request);
        }
    }
}
