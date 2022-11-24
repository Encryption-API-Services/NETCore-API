using System;
using System.Collections.Generic;
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
            string hashed = this._wrapper.HashString(this._testString);
            Assert.NotNull(hashed);
            Assert.NotEmpty(hashed);
        }

        [Fact]
        public async Task HashPasswordAsync()
        {
            string hashed = await this._wrapper.HashPasswordAsync(this._testString);
            Assert.NotNull(hashed);
            Assert.NotEmpty(hashed);
        }

        
    }
}
