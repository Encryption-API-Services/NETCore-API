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

        public async Task<User> GetUserById(string id)
        {
            return await this._userCollection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this._userCollection.FindAsync(x => x.Email == email).Result.FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetUsersMadeWithinLastThirtyMinutes()
        {
            DateTime now = DateTime.UtcNow;
            return await this._userCollection.FindAsync(x => x.IsActive == false &&
                                                        x.CreationTime < now && x.CreationTime > now.AddMinutes(-30)
                                                        && x.EmailActivationToken == null).Result.ToListAsync();
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
        public async Task UpdateUsersRsaKeyPairsAndToken(User user, string pubXml, string privateXml, string token, byte[] signedToken)
        {
            EmailActivationToken emailToken = new EmailActivationToken()
            {
                PublicKey = pubXml,
                PrivateKey = privateXml,
                SignedToken = signedToken,
                Token = token
            };
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.EmailActivationToken, emailToken);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task ChangeUserActiveById(User user, bool isActive)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.IsActive, isActive);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            User userToReturn = null;
            User user = await this._userCollection.FindAsync(x => x.Email == email && x.IsActive == true).Result.FirstOrDefaultAsync();
            BcryptWrapper bcryptWrapper = new BcryptWrapper();
            if (await bcryptWrapper.Verify(user.Password, password))
            {
                userToReturn = user;
            }
            return userToReturn;
        }

        public async Task UpdateUsersJwtToken(User user, JwtToken token)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.JwtToken, token);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateForgotPassword(string userId, ForgotPassword forgotPassword)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.ForgotPassword, forgotPassword);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<List<User>> GetUsersWhoForgotPassword()
        {
            return await this._userCollection.FindAsync(x => x.ForgotPassword != null && x.ForgotPassword.Token != null && x.ForgotPassword.HasBeenReset == false).GetAwaiter().GetResult().ToListAsync();
        }
    }
}