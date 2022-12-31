using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
            string encrypted = Marshal.PtrToStringUTF8(this._aESWrapper.EncryptPerformant(this._key, toEncrypt));
            AESWrapper.free_aes_string();
            Assert.NotEqual(toEncrypt, encrypted);
        }

        [Fact]
        public async Task EncryptPerformantAsync()
        {
            string toEncrypt = "Text to Asyn up";
            string encrypted = Marshal.PtrToStringUTF8(await this._aESWrapper.EncryptPerformantAsync(this._key, toEncrypt));
            AESWrapper.free_aes_string();
            Assert.NotEqual(toEncrypt, encrypted);
        }

        [Fact]
        public void DecryptPerformant()
        {
            string toEncrypt = "Text to encrypt";
            string encrypted = Marshal.PtrToStringUTF8(this._aESWrapper.EncryptPerformant(this._key, toEncrypt));
            AESWrapper.free_aes_string();
            string decrypted = Marshal.PtrToStringUTF8(this._aESWrapper.DecryptPerformant(this._key, encrypted));
            AESWrapper.free_aes_string();
            Assert.Equal(toEncrypt, decrypted);
        }

        [Fact]
        public async Task DecryptPerformantAsync()
        {
            string toEncrypt = "Text to async up to the moon!";
            string encrypted = Marshal.PtrToStringUTF8(await this._aESWrapper.EncryptPerformantAsync(this._key, toEncrypt));
            AESWrapper.free_aes_string();
            string decrypted = Marshal.PtrToStringUTF8(await this._aESWrapper.DecryptPerformantAsync(this._key, encrypted));
            AESWrapper.free_aes_string();
            Assert.Equal(toEncrypt, decrypted);
        }
    }
}
