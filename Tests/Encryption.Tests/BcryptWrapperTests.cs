using Encryption.PasswordHash;
using Models.UserAuthentication;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class BcryptWrapperTests
    {
        private BcryptWrapper _cryptWrapper { get; set; }
        private string _testPassword { get; set; }

        public BcryptWrapperTests()
        {
            this._cryptWrapper = new BcryptWrapper();
            this._testPassword = "testPassword";
        }

        [Fact]
        public void HashPassword()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(this._cryptWrapper.HashPassword(this._testPassword));
            BcryptWrapper.free_bcrypt_string();
            Assert.NotEqual(hashedPassword, this._testPassword);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(await this._cryptWrapper.HashPasswordAsync(this._testPassword));
            BcryptWrapper.free_bcrypt_string();
            Assert.NotEqual(hashedPassword, this._testPassword);
        }

        [Fact]
        public async Task Verify()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(await this._cryptWrapper.HashPasswordAsync(this._testPassword));
            BcryptWrapper.free_bcrypt_string();
            Assert.Equal(await this._cryptWrapper.Verify(hashedPassword, this._testPassword), true);
        }

        [Fact]
        public async Task VerifyAsync()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(await this._cryptWrapper.HashPasswordAsync(this._testPassword));
            BcryptWrapper.free_bcrypt_string();
            Assert.Equal(await this._cryptWrapper.VerifyAsync(hashedPassword, this._testPassword), true);
        }
    }
}
