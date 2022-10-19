using Models.UserAuthentication;
using System;
using System.Collections.Generic;
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
    }
}