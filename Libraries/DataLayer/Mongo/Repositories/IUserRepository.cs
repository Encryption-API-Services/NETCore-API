﻿using DataLayer.Mongo.Entities;
using Models.UserAuthentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IUserRepository
    {
        public Task AddUser(RegisterUser model);
        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserByEmailAndPassword(string email, string password);
        public Task<List<User>> GetUsersMadeWithinLastThirtyMinutes();
        public Task<User> GetUserById(string id);
        public Task ChangeUserActiveById(User user, bool isActive);
        public Task UpdateUsersJwtToken(User user, JwtToken token);
        public Task UpdateForgotPassword(string userId, ForgotPassword forgotPassword);
        public Task LockoutUser(string userId);
    }
}
