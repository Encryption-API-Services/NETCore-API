using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class ED25519WrapperTests
    {
        private readonly ED25519Wrapper _ed25519;

        public ED25519WrapperTests()
        {
            this._ed25519 = new ED25519Wrapper();
        }

        [Fact]
        public async Task TestSignature()
        {
            string dataToSign = "Data to sign";
            byte[] utf8DataToSign = Encoding.UTF8.GetBytes(dataToSign);
            await this._ed25519.CreateKeyPair();
            byte[] signedData = await this._ed25519.SignDataAsync(utf8DataToSign);
            Assert.NotNull(signedData);
            Assert.NotEqual(utf8DataToSign, signedData);
        }
    }
}