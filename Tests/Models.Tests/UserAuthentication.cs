using Models.UserAuthentication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace Models.Tests
{
    public class UserAuthentication
    {
        private string _userName = "testuseraccount";
        private string _password = "testpassword12345";

        [Fact]
        public void CreateRegisterUserModel()
        {
            RegisterUser user = new RegisterUser()
            {
                username = this._userName,
                password = this._password
            };

            Assert.Equal(this._userName, user.username);
            Assert.Equal(this._password, user.password);
        }

        [Fact]
        public void CreateActivateUser()
        {
            string id = Guid.NewGuid().ToString();
            string token = Guid.NewGuid().ToString();

            ActivateUser user = new ActivateUser()
            {
                Id = id,
                Token = token
            };

            Assert.Equal(user.Id, id);
            Assert.Equal(user.Token, token);
        }

        [Fact]
        public void CreateForgotPasswordRequest()
        {
            ForgotPasswordRequest request = new ForgotPasswordRequest()
            {
                Email = "testing@outlook.com"
            };

            Assert.NotNull(request);
            Assert.NotNull(request.Email);
        }
    }
}