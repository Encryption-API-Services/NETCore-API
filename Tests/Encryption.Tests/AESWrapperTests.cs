using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class AESWrapperTests
    {
        private readonly AESWrapper _aESWrapper;
        private readonly string _key;

        public AESWrapperTests()
        {
            this._aESWrapper = new AESWrapper();
            this._key = "My Test Key";
        }

        [Fact]
        public void EncryptPerformant()
        {
            string toEncrypt = "Text to encrypt";
            string encrypted = this._aESWrapper.EncryptPerformant(this._key, toEncrypt);
            Assert.NotEqual(toEncrypt, encrypted);
        }

        [Fact]
        public async Task EncryptPerformantAsync()
        {
            string toEncrypt = "Text to Asyn up";
            string encrypted = await this._aESWrapper.EncryptPerformantAsync(this._key, toEncrypt);
            Assert.NotEqual(toEncrypt, encrypted);
        }

        [Fact]
        public void DecryptPerformant()
        {
            string toEncrypt = "Text to encrypt";
            string encrypted = this._aESWrapper.EncryptPerformant(this._key, toEncrypt);
            string decrypted = this._aESWrapper.DecryptPerformant(this._key, encrypted);
            Assert.Equal(toEncrypt, decrypted);
        }

        [Fact]
        public async Task DecryptPerformantAsync()
        {
            string toEncrypt = "Text to async up to the moon!";
            string encrypted = await this._aESWrapper.EncryptPerformantAsync(this._key, toEncrypt);
            string decrypted = await this._aESWrapper.DecryptPerformantAsync(this._key, encrypted);
            Assert.Equal(toEncrypt, decrypted);
        }
    }
}
