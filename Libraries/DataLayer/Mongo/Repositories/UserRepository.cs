using Common.UniqueIdentifiers;
using DataLayer.Mongo.Entities;
using Encryption;
using Models.UserAuthentication;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Types;

namespace DataLayer.Mongo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly BcryptWrapper _bCrypt;

        public UserRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.Connection);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._userCollection = database.GetCollection<User>("Users");
            this._bCrypt = new BcryptWrapper();
        }
        public async Task AddUser(RegisterUser model)
        {
            await this._userCollection.InsertOneAsync(new User
            {
                Username = model.username,
                Password = await this._bCrypt.HashPasswordPerformantAsync(model.password),
                Email = model.email,
                IsActive = false,
                Phone2FA = new Phone2FA()
                {
                    PhoneNumber = null,
                    IsEnabled = false
                },
                CreationTime = DateTime.UtcNow,
                LastModifiedTime = DateTime.UtcNow,
                LockedOut = new LockedOut() { IsLockedOut = false, HasBeenSentOut = false },
                ApiKey = new Generator().CreateApiKey()
            });
        }

        public async Task<User> GetUserById(string id)
        {
            return await this._userCollection.FindAsync(x => x.Id == id).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByIdAndPublicKey(string id, string publicKey)
        {
            return await this._userCollection.FindAsync(x => x.Id == id && x.JwtToken.PublicKey == publicKey).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this._userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
        }
        public async Task<List<User>> GetUsersMadeWithinLastThirtyMinutes()
        {
            DateTime now = DateTime.UtcNow;
            return await this._userCollection.FindAsync(x => x.IsActive == false &&
                                                        x.CreationTime < now && x.CreationTime > now.AddMinutes(-30)
                                                        && x.EmailActivationToken == null).Result.ToListAsync();
        }
        // TODO: remove Testing()
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

        public async Task ChangeUserActiveById(User user, bool isActive, string stripCustomerId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update.Set(x => x.IsActive, isActive)
                                              .Set(x => x.StripCustomerId, stripCustomerId);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            User userToReturn = null;
            User user = await this._userCollection.Find(x => x.Email == email && x.IsActive == true).FirstOrDefaultAsync();
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

        public async Task UpdatePassword(string userId, string password)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.Password, password);
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
            return await this._userCollection.FindAsync(x => x.ForgotPassword != null &&
                                                            x.ForgotPassword.Token != null &&
                                                            x.ForgotPassword.PrivateKey == null &&
                                                            x.ForgotPassword.PublicKey == null &&
                                                            x.ForgotPassword.HasBeenReset == false).GetAwaiter().GetResult().ToListAsync();
        }

        public async Task UpdateUsersForgotPasswordToReset(string userId, string forgotPasswordToken, string publicKey, string privateKey, byte[] signedToken)
        {
            ForgotPassword forgotPassword = new ForgotPassword()
            {
                Token = forgotPasswordToken,
                PublicKey = publicKey,
                PrivateKey = privateKey,
                SignedToken = signedToken,
                HasBeenReset = true
            };

            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.ForgotPassword, forgotPassword);
            await this._userCollection.UpdateOneAsync(filter, update);
        }
        public async Task LockoutUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.LockedOut.IsLockedOut, true);
            await this._userCollection.UpdateOneAsync(filter, update);
        }
        public async Task<List<User>> GetLockedOutUsers()
        {
            return await this._userCollection.Find(x => x.LockedOut.IsLockedOut == true && x.LockedOut.HasBeenSentOut == false).ToListAsync();
        }
        public async Task UpdateUserLockedOutToSentOut(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.LockedOut.HasBeenSentOut, true);
            await this._userCollection.UpdateOneAsync(filter, update);
        }
        public async Task UnlockUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.LockedOut.IsLockedOut, false)
                                              .Set(x => x.LockedOut.HasBeenSentOut, false);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await this._userCollection.Find(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task<Phone2FA> GetPhone2FAStats(string userId)
        {
            return await this._userCollection.AsQueryable().Where(x => x.Id == userId).Select(x => x.Phone2FA).FirstOrDefaultAsync();
        }

        public async Task ChangePhone2FAStatusToEnabled(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.Phone2FA.IsEnabled, true);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task ChangePhone2FAStatusToDisabled(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.Phone2FA.IsEnabled, false);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task ChangePhoneNumberByUserID(string userId, string phoneNumber)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.Phone2FA.PhoneNumber, phoneNumber);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<string> GetPhoneNumberByUserId(string userId)
        {
            return await this._userCollection.AsQueryable().Where(x => x.Id == userId).Select(x => x.Phone2FA.PhoneNumber).FirstOrDefaultAsync();
        }

        public async Task AddCardToUser(string userId, string cardId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Set(x => x.StripCardId, cardId);
            await this._userCollection.UpdateOneAsync(filter, update);
        }

        public async Task<string> GetApiKeyById(string userId)
        {
            return await this._userCollection.AsQueryable().Where(x => x.Id == userId).Select(x => x.ApiKey).FirstOrDefaultAsync();
        }
    }
}