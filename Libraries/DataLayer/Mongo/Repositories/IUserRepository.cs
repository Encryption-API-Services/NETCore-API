using DataLayer.Mongo.Entities;
using Models.UserAuthentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IUserRepository
    {
        public Task AddUser(RegisterUser model, string hashedPassword);
        public Task<User> GetUserByEmail(string email);
        public Task<User> GetUserByUsername(string username);
        public Task<User> GetUserByEmailAndPassword(string email, string password);
        public Task<List<User>> GetUsersMadeWithinLastThirtyMinutes();
        public Task<User> GetUserById(string id);
        public Task ChangeUserActiveById(User user, bool isActive, string stripCustomerId);
        public Task UpdatePassword(string userId, string password);
        public Task LockoutUser(string userId);
        public Task UpdateForgotPassword(string userId, ForgotPassword forgotPassword);
        public Task<List<User>> GetLockedOutUsers();
        public Task<List<User>> GetUsersWhoForgotPassword();
        public Task UpdateUsersForgotPasswordToReset(string userId, string forgotPasswordToken, string publicKey, string privateKey, byte[] signedToken);
        public Task UpdateUserLockedOutToSentOut(string userId);
        public Task UnlockUser(string userId);
        public Task<Phone2FA> GetPhone2FAStats(string userId);
        public Task ChangePhone2FAStatusToEnabled(string userId);
        public Task ChangePhone2FAStatusToDisabled(string userId);
        public Task ChangePhoneNumberByUserID(string userId, string phoneNumber);
        public Task<string> GetPhoneNumberByUserId(string userId);
        public Task AddCardToUser(string userId, string cardId);
        public Task<string> GetApiKeyById(string userId);
        public Task<User> GetUserByApiKey(string apiKey);
    }
}
