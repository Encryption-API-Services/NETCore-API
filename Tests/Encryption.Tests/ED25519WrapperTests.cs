using Org.BouncyCastle.Crypto.Paddings;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class ED25519WrapperTests
    {
        private readonly ED25519Wrapper _ed25519;
        private byte[] _signature;
        private byte[] _dataToSign;

        public ED25519WrapperTests()
        {
            this._ed25519 = new ED25519Wrapper();
            this._ed25519.CreateKeyPair().GetAwaiter().GetResult();
            this._dataToSign = Encoding.UTF8.GetBytes("Data To Sign");
        }

        [Fact]
        public async Task TestSignature()
        {
            string dataToSign = "Data to sign";
            byte[] utf8DataToSign = Encoding.UTF8.GetBytes(dataToSign);
            this._signature = await this._ed25519.SignDataAsync(utf8DataToSign);
            Assert.NotNull(this._signature);
            Assert.NotEqual(utf8DataToSign, this._signature);
        }

        [Fact]
        public async Task TestVerifySignature()
        {
            ED25519Wrapper ed25519 = new ED25519Wrapper();
            await ed25519.CreateKeyPair();
            byte[] dataToSign = Encoding.UTF8.GetBytes("Data to Sign for this function");
            byte[] signedData = await ed25519.SignDataAsync(dataToSign);
            bool result = await this._ed25519.VerifySignatureAsync(ed25519.key.PublicKey, dataToSign, signedData);
            Assert.Equal(result, true);
        }
    }
}