using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;
using static Encryption.ED25519Wrapper;

namespace Encryption.Tests
{
    public class ED25519WrapperTests
    {
        private readonly ED25519Wrapper _wrapper;
        public ED25519WrapperTests()
        {
            this._wrapper = new ED25519Wrapper();
        }

        [Fact]
        public void GetKeyPair()
        {
            string keyPair = this._wrapper.GetKeyPair();
            Assert.NotNull(keyPair);
        }

        [Fact]
        public async Task GetKeyPairAsync()
        {
            string keyPair = await this._wrapper.GetKeyPairAsync();
            Assert.NotNull(keyPair);
        }

        [Fact]
        public void SignData()
        {
            string keyPair = this._wrapper.GetKeyPair();
            Ed25519SignatureResult signedData = this._wrapper.Sign(keyPair, "SignThisData");
            Assert.NotNull(signedData.Signature);
            Assert.NotNull(signedData.Public_Key);
        }

        [Fact]
        public async Task SignDataAsync()
        {
            string keyPair = await this._wrapper.GetKeyPairAsync();
            Ed25519SignatureResult signedData = this._wrapper.Sign(keyPair, "SignThisData");
            Assert.NotNull(signedData.Signature);
            Assert.NotNull(signedData.Public_Key);
        }

        [Fact]
        public void Verify()
        {
            string keyPair = this._wrapper.GetKeyPair();
            string dataToSign = "TestData12345";
            Ed25519SignatureResult signature = this._wrapper.Sign(keyPair, dataToSign);
            bool isValid = this._wrapper.Verify(keyPair, signature.Signature, dataToSign);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyAsync()
        {
            string keyPair = await this._wrapper.GetKeyPairAsync();
            string dataToSign = "TestData12345";
            Ed25519SignatureResult signature = await this._wrapper.SignAsync(keyPair, dataToSign);
            bool isValid = await this._wrapper.VerifyAsync(keyPair, signature.Signature, dataToSign);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async void VerifyWithPublicKey()
        {
            string keyPair =  this._wrapper.GetKeyPair();
            string dataToSign = "welcomeHome";
            Ed25519SignatureResult result = this._wrapper.Sign(keyPair, dataToSign);
            bool isValid = this._wrapper.VerifyWithPublicKey(result.Public_Key, result.Signature, dataToSign);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyWithPublicAsync()
        {
            string keyPair = await this._wrapper.GetKeyPairAsync();
            string dataToSign = "welcomeHome";
            Ed25519SignatureResult result = await this._wrapper.SignAsync(keyPair, dataToSign);
            bool isValid = await this._wrapper.VerifyWithPublicAsync(result.Public_Key, result.Signature, dataToSign);
            Assert.Equal(true, isValid);
        }
    }
}