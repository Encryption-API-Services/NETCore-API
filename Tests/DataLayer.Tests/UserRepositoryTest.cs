using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption.PasswordHash;
using Models.UserAuthentication;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DataLayer.Tests
{
    public class UserRepositoryTest
    {
        private readonly IUserRepository _userRepository;
        private readonly RegisterUser _registerUser;
        public UserRepositoryTest()
        {
            this._userRepository = new UserRepository(new DatabaseSettings
            {
                Connection = Environment.GetEnvironmentVariable("Connection"),
                DatabaseName = Environment.GetEnvironmentVariable("Database"),
                UserCollectionName = Environment.GetEnvironmentVariable("UserCollectionName")
            });
            this._registerUser = new RegisterUser
            {
                username = "testUser1234",
                email = "testingemail@outlook.com",
                password = "Testing1234@#$1!"
            };
        }

        [Fact]
        public async Task AddUserTest()
        {
            Argon2Wrappper argon2 = new Argon2Wrappper();
            string hashedPassword = await argon2.HashPasswordAsync("DoNotUseThisPassword");
            await this._userRepository.AddUser(this._registerUser, hashedPassword);
            User databaseUser = await this._userRepository.GetUserByEmail(this._registerUser.email);
            Assert.NotNull(databaseUser);
        }

        [Fact] 
        public async Task GetUserByEmail()
        {
            User databaseUser = await this._userRepository.GetUserByEmail(this._registerUser.email);
            Assert.NotNull(databaseUser);
        }
    }
}