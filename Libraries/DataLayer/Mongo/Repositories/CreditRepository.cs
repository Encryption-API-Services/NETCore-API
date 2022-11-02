using DataLayer.Mongo.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class CreditRepository : ICreditRepository
    {
        private readonly IMongoCollection<ValidatedCreditCard> _validatedCreditCards;
        public CreditRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._validatedCreditCards = database.GetCollection<ValidatedCreditCard>("ValidateCreditCards");
        }

        public async Task AddValidatedCreditInformation(ValidatedCreditCard card)
        {
            await this._validatedCreditCards.InsertOneAsync(card);
        }
    }
}
