using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System;
using System.Threading.Tasks;
using UsersAPI.Config;
using Xunit;

namespace UsersAPI.Tests
{
    public class RegisterUserController
    {
        private readonly UserRegisterController _controller;
        public RegisterUserController()
        {
            this._controller = new UserRegisterController(new UserRegisterControllerLogic(new UserRepository(new DatabaseSettings()
            {
                Connection = Environment.GetEnvironmentVariable("Connection"),
                DatabaseName = Environment.GetEnvironmentVariable("Database"),
                UserCollectionName = Environment.GetEnvironmentVariable("UserCollectionName")
            })));
        }


        [Fact]
        public async Task RegisterNewUserSuccess()
        {
            Random random = new Random();
            var digitOne = random.Next();
            var digitTwo = random.Next();

            RegisterUser newUser = new RegisterUser()
            {
                email = String.Format("mtmulch{0}{1}@outlook.com", digitOne, digitTwo),
                password = "Testing123456!@",
                username = String.Format("mtmulch{0}{1}", digitOne, digitTwo)
            };
            IActionResult response = await this._controller.Post(newUser);
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task RegisterNewUserFailure()
        {
            RegisterUser user = new RegisterUser()
            {
                email = "email@testing.com",
                password = "testing123@",
                username = "welcomehome"
            };
            IActionResult response = await this._controller.Post(user);
            IActionResult response2 = await this._controller.Post(user);
            Assert.IsType<BadRequestResult>(response2);
        }
    }
}