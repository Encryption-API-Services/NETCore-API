using Models.Encryption;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Models.Tests
{
    public class Encryption
    {
        public Encryption()
        {

        }

        [Fact]
        public void CreateBCryptModel()
        {
            BCryptEncryptModel model = new BCryptEncryptModel()
            {
                Password = "testpassword"
            };
            Assert.NotNull(model);
            Assert.NotNull(model.Password);
        }

        [Fact]
        public void CreateBcryptVerifyModel()
        {
            BcryptVerifyModel model = new BcryptVerifyModel()
            {
                Password = "testpassword",
                ID = Guid.NewGuid().ToString()
            };
            Assert.NotNull(model);
            Assert.NotNull(model.Password);
            Assert.NotNull(model.ID);
        }

        [Fact]
        public void CreateDecryptAESRequest()
        {
            DecryptAESRequest model = new DecryptAESRequest()
            {
                Data = "Data to encryption data",
                Key = "testpassword",
                IV = Guid.NewGuid().ToString()
            };
            Assert.NotNull(model);
            Assert.NotNull(model.Key);
            Assert.NotNull(model.IV);
            Assert.NotNull(model.Data);
        }

        [Fact]
        public void CreateEncryptSHARequest()
        {
            EncryptSHARequest model = new EncryptSHARequest()
            {
                DataToEncrypt = "testpassword",
                
            };
            Assert.NotNull(model.DataToEncrypt);
        }

        [Fact]
        public void CreateEncryptAESRequest()
        {
            EncryptAESRequest model = new EncryptAESRequest()
            {
                DataToEncrypt = "sha that data bad boy"
            };
            Assert.NotNull(model);
            Assert.NotNull(model.DataToEncrypt);
        }
    }
}
