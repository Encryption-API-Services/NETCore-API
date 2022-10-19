using DataLayer.Mongo.Entities;
using Encryption;
using Microsoft.Extensions.Options;
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

        public UserRepository(IUserDatabaseSettings databaseSettings)
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
    }
}
