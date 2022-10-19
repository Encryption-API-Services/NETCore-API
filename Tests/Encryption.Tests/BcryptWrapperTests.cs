﻿using Models.UserAuthentication;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class BcryptWrapperTests
    {
        private BcryptWrapper _cryptWrapper { get; set; }
        private string _testPassword { get; set; }

        public BcryptWrapperTests()
        {
            this._cryptWrapper = new BcryptWrapper();
            this._testPassword = "testPassword";
        }

        [Fact]
        public void HashPassword()
        {
            string hashedPassword = this._cryptWrapper.HashPassword(this._testPassword);
            Assert.NotEqual(hashedPassword, this._testPassword);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashedPassword = await this._cryptWrapper.HashPasswordAsync(this._testPassword);
            Assert.NotEqual(hashedPassword, this._testPassword);
        }
    }
}
