using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class CreditCardInfoChangedRepository : ICreditCardInfoChangedRepository
    {
        private readonly IMongoCollection<CreditCardInfoChanged> _collection;
        public CreditCardInfoChangedRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._collection = database.GetCollection<CreditCardInfoChanged>("CreditCardInfoChanged");
        }
        public async Task InsertCreditCardInformationChanged(CreditCardInfoChanged changedInfo)
        {
            await this._collection.InsertOneAsync(changedInfo);
        }
    }
}