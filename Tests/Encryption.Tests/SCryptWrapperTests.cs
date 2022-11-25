using System.Text;
using System.Threading.Tasks;
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
            string hashedPassword = this._scrypt.HashPassword(this._password);
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashedPassword = await this._scrypt.HashPasswordAsync(this._password);
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(hashedPassword, this._password);
        }

        [Fact] 
        public void VerifyPassword()
        {
            string hashedPassword = this._scrypt.HashPassword(this._password);
            bool isValid = this._scrypt.VerifyPassword(this._password, hashedPassword);
            Assert.Equal(true, isValid);
        }

        [Fact]
        public async Task VerifyPasswordAsync()
        {
            string hashedPassword = this._scrypt.HashPassword(this._password);
            bool isValid = await this._scrypt.VerifyPasswordAsync(this._password, hashedPassword);
            Assert.Equal(true, isValid);
        }
    }
}
