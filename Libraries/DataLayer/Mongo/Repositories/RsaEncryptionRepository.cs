using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class RsaEncryptionRepository : IRsaEncryptionRepository
    {
        private readonly IMongoCollection<RsaEncryption> _rsaEncryptions;
        public RsaEncryptionRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._rsaEncryptions = database.GetCollection<RsaEncryption>("RsaEncryptions");
        } 
        public async Task InsertNewEncryption(RsaEncryption newEncryption)
        {
            await this._rsaEncryptions.InsertOneAsync(newEncryption);
        }
    }
}