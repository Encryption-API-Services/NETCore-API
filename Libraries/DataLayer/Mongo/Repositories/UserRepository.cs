using DataLayer.Mongo.Entities;
using Encryption;
using Models.UserAuthentication;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly BcryptWrapper _bcryptWrapper;

        public UserRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._userCollection = database.GetCollection<User>("Users");
            this._bcryptWrapper = new BcryptWrapper();
        }
        public async Task AddUser(RegisterUser model)
        {
            await this._userCollection.InsertOneAsync(new User
            {
                Username = model.username,
                Password = await this._bcryptWrapper.HashPasswordAsync(model.password),
                Email = model.email,
                IsActive = false,
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow
            });
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await this._userCollection.FindAsync(x => x.Email == email).Result.FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetUsersMadeWithinLastThirtyMinutes()
        {
            DateTime now = DateTime.UtcNow;
            return await this._userCollection.FindAsync(x => x.IsActive == false && x.CreationTime < now && x.CreationTime > now.AddMinutes(-30)).Result.ToListAsync();
        }

        public async Task Testing()
        {
            DateTime now = DateTime.UtcNow;
            var filter = Builders<User>.Filter.Eq(x => x.IsActive, false)
                & Builders<User>.Filter.Lt(x => x.CreationTime, now)
                & Builders<User>.Filter.Gt(x => x.CreationTime, now.AddMinutes(-30));

            var updateDefintion = Builders<User>.Update.Set(x => x.IsActive, true);
            await this._userCollection.UpdateOneAsync(filter, updateDefintion);

        }
        public async Task UpdateUsersRsaKeyPairs(User user, string pubXml, string privateXml)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.PrivateKey, privateXml)
                .Set(x => x.PublicKey, pubXml);
            await this._userCollection.UpdateOneAsync(filter, update);         
        }
    }
}