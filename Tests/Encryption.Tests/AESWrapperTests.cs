using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Encryption.AESWrapper;

namespace Encryption.Tests
{
    public class AESWrapperTests
    {
        private readonly AESWrapper _aESWrapper;
        private readonly string _key;

        public AESWrapperTests()
        {
            this._aESWrapper = new AESWrapper();
        }

        [Fact]
        public void EncryptPerformant()
        {
            string nonceKey = "TestingNonce";
            string toEncrypt = "Text to encrypt";
            AesEncrypt encrypted = this._aESWrapper.EncryptPerformant(nonceKey, toEncrypt);
            Assert.NotEqual(toEncrypt, encrypted.ciphertext);
        }

        [Fact]
        public async Task EncryptPerformantAsync()
        {
            string nonceKey = "TestingNonce";
            string toEncrypt = "Text to Asyn up";
            AesEncrypt encrypted = await this._aESWrapper.EncryptPerformantAsync(nonceKey, toEncrypt);
            Assert.NotEqual(toEncrypt, encrypted.ciphertext);
        }

        [Fact]
        public void DecryptPerformant()
        {
            string nonceKey = "TestingNonce";
            string toEncrypt = "Text to encrypt";
            AesEncrypt encrypted = this._aESWrapper.EncryptPerformant(nonceKey, toEncrypt);
            string decrypted = this._aESWrapper.DecryptPerformant(nonceKey, encrypted.key, encrypted.ciphertext);
            Assert.Equal(toEncrypt, decrypted);
        }

        [Fact]
        public async Task DecryptPerformantAsync()
        {
            string nonceKey = "TestingNonce";
            string toEncrypt = "Text to encrypt";
            AesEncrypt encrypted = await this._aESWrapper.EncryptPerformantAsync(nonceKey, toEncrypt);
            string decrypted = await this._aESWrapper.DecryptPerformantAsync(nonceKey, encrypted.key, encrypted.ciphertext);
            Assert.Equal(toEncrypt, decrypted);
        }
    }
}
