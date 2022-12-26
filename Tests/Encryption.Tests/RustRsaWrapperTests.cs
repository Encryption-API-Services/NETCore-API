using System.Threading.Tasks;
using Xunit;
using static Encryption.RustRSAWrapper;

namespace Encryption.Tests
{
    public class RustRsaWrapperTests
    {
        private readonly RustRSAWrapper _rustRsaWrapper;
        private readonly RustRsaKeyPair _encryptDecryptKeyPair;

        public RustRsaWrapperTests()
        {
            this._rustRsaWrapper = new RustRSAWrapper();
            this._encryptDecryptKeyPair = this._rustRsaWrapper.GetKeyPairAsync(4096).GetAwaiter().GetResult();
        }

        [Fact]
        public void CreateKeyPair()
        {
            RustRsaKeyPair keyPair = this._rustRsaWrapper.GetKeyPair(4096);
            Assert.NotNull(keyPair.priv_key);
            Assert.NotNull(keyPair.pub_key);
        }

        [Fact]
        public async Task CreateKeyPairAsync()
        {
            RustRsaKeyPair keyPair = await this._rustRsaWrapper.GetKeyPairAsync(4096);
            Assert.NotNull(keyPair.priv_key);
            Assert.NotNull(keyPair.pub_key);
        }

        [Fact]
        public void RsaEncrypt()
        {
            string dataToEncrypt = "EncryptingStuffIsFun";
            string encrypted = this._rustRsaWrapper.RsaEncrypt(this._encryptDecryptKeyPair.pub_key, dataToEncrypt);
            Assert.NotEqual(dataToEncrypt, encrypted);
        }

        [Fact]
        public async Task RsaEncryptAsync()
        {
            string dataToEncrypt = "EncryptingStuffIsFun";
            string encrypted = await this._rustRsaWrapper.RsaEncryptAsync(this._encryptDecryptKeyPair.pub_key, dataToEncrypt);
            Assert.NotEqual(dataToEncrypt, encrypted);
        }

        [Fact]
        public void RsaDecrypt()
        {
            string dataToEncrypt = "EncryptingStuffIsFun";
            string encrypted = this._rustRsaWrapper.RsaEncrypt(this._encryptDecryptKeyPair.pub_key, dataToEncrypt);
            string decrypted = this._rustRsaWrapper.RsaDecrypt(this._encryptDecryptKeyPair.priv_key, encrypted);
            Assert.Equal(dataToEncrypt, decrypted);
        }

        [Fact]
        public async Task RsaDecryptAsync()
        {
            string dataToEncrypt = "EncryptingStuffIsFun";
            string encrypted = await this._rustRsaWrapper.RsaEncryptAsync(this._encryptDecryptKeyPair.pub_key, dataToEncrypt);
            string decrypted = await this._rustRsaWrapper.RsaDecryptAsync(this._encryptDecryptKeyPair.priv_key, encrypted);
            Assert.Equal(dataToEncrypt, decrypted);
        }
    }
}