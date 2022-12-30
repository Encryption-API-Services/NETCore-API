using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Encryption.PasswordHash;
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
            string hash = Marshal.PtrToStringUTF8(this._argon2Wrapper.HashPassword(password));
            Argon2Wrappper.free_argon2_string();
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string password = "DoNotUSETHISPASS@!";
            string hash = Marshal.PtrToStringUTF8(await this._argon2Wrapper.HashPasswordAsync(password));
            Argon2Wrappper.free_argon2_string();
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void Verify()
        {
            string password = "TestPasswordToVerify";
            string hash = Marshal.PtrToStringUTF8(this._argon2Wrapper.HashPassword(password));
            bool isValid = this._argon2Wrapper.VerifyPassword(hash, password);
            Argon2Wrappper.free_argon2_string();
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyAsync()
        {
            string password = "AsyncTestingToTheMoon!";
            string hash = Marshal.PtrToStringUTF8(await this._argon2Wrapper.HashPasswordAsync(password));
            bool isValid = await this._argon2Wrapper.VerifyPasswordAsync(hash, password);
            Argon2Wrappper.free_argon2_string();
            Assert.NotEqual(false, isValid);
        }
    }
}
