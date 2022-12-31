using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Encryption.PasswordHash;
using Xunit;

namespace Encryption.Tests
{
    public class SCryptWrapperTests
    {
        private readonly SCryptWrapper _scrypt;
        private readonly string _password;
        public SCryptWrapperTests()
        {
            this._scrypt = new SCryptWrapper();
            this._password = "TestPasswordToHash";
        }

        [Fact]
        public void HashPassword()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(this._scrypt.HashPassword(this._password));
            SCryptWrapper.free_scrypt_string();
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(await this._scrypt.HashPasswordAsync(this._password));
            SCryptWrapper.free_scrypt_string();
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact] 
        public void VerifyPassword()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(this._scrypt.HashPassword(this._password));
            SCryptWrapper.free_scrypt_string();
            bool isValid = this._scrypt.VerifyPassword(this._password, hashedPassword);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyPasswordAsync()
        {
            string hashedPassword = Marshal.PtrToStringUTF8(this._scrypt.HashPassword(this._password));
            SCryptWrapper.free_scrypt_string();
            bool isValid = await this._scrypt.VerifyPasswordAsync(this._password, hashedPassword);
            Assert.Equal(true, isValid);
        }
    }
}
