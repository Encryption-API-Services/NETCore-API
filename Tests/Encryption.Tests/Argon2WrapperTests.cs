using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class Argon2WrapperTests
    {
        private Argon2Wrappper _argon2Wrapper;

        public Argon2WrapperTests()
        {
            this._argon2Wrapper = new Argon2Wrappper();
        }

        [Fact]
        public void HashPassword()
        {
            string password = "DoNotUSETHISPASS@!";
            string hash = this._argon2Wrapper.HashPassword(password);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string password = "DoNotUSETHISPASS@!";
            string hash = await this._argon2Wrapper.HashPasswordAsync(password);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void Verify()
        {
            string password = "TestPasswordToVerify";
            string hash = this._argon2Wrapper.HashPassword(password);
            bool isValid = this._argon2Wrapper.VerifyPassword(hash, password);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyAsync()
        {
            string password = "AsyncTestingToTheMoon!";
            string hash = await this._argon2Wrapper.HashPasswordAsync(password);
            bool isValid = await this._argon2Wrapper.VerifyPasswordAsync(hash, password);
            Assert.NotEqual(false, isValid);
        }
    }
}
