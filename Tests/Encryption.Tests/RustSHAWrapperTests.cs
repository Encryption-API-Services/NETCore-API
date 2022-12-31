using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class RustSHAWrapperTests
    {
        private RustSHAWrapper _wrapper;
        private string _testString;
        public RustSHAWrapperTests()
        {
            this._wrapper = new RustSHAWrapper();
            this._testString = "Test hash to hash";
        }

        [Fact]
        public void HashPassword()
        {
            string hashed = Marshal.PtrToStringUTF8(this._wrapper.HashString(this._testString));
            RustSHAWrapper.free_sha_string();
            Assert.NotNull(hashed);
            Assert.NotEmpty(hashed);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashed = Marshal.PtrToStringUTF8(await this._wrapper.HashPasswordAsync(this._testString));
            RustSHAWrapper.free_sha_string();
            Assert.NotNull(hashed);
            Assert.NotEmpty(hashed);
        }

        [Fact]
        public async Task VerifyPassword()
        {
            string hashed = Marshal.PtrToStringUTF8(await this._wrapper.HashPasswordAsync(this._testString));
            RustSHAWrapper.free_sha_string();
            bool verified = this._wrapper.VerifyHash(this._testString, hashed);
            Assert.Equal(true, verified);
        }

        [Fact]
        public async Task VerifyPasswordAsync()
        {
            string hashed = Marshal.PtrToStringUTF8(await this._wrapper.HashPasswordAsync(this._testString));
            RustSHAWrapper.free_sha_string();
            bool verified = await this._wrapper.VerifyHashAsync(this._testString, hashed);
            Assert.Equal(true, verified);
        }
    }
}
