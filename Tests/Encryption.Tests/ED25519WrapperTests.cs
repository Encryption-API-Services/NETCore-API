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
            Ed25519KeyPair keyPair = this._wrapper.GetKeyPair();
            Assert.NotNull(keyPair.private_key);
            Assert.NotNull(keyPair.public_key);
        }

        [Fact]
        public async Task GetKeyPairAsync()
        {
            Ed25519KeyPair keyPair = await this._wrapper.GetKeyPairAsync();
            Assert.NotNull(keyPair.private_key);
            Assert.NotNull(keyPair.public_key);
        }
    }
}