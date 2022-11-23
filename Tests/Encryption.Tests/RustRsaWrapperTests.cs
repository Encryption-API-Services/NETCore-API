using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Encryption.RustRSAWrapper;

namespace Encryption.Tests
{
    public class RustRsaWrapperTests
    {
        private readonly RustRSAWrapper _rustRsaWrapper;

        public RustRsaWrapperTests()
        {
            this._rustRsaWrapper = new RustRSAWrapper();
        }

        [Fact]
        public void CreateKeyPair()
        {
            RustRsaKeyPair keyPair = this._rustRsaWrapper.GetKeyPair(4096);
            Assert.NotNull(keyPair.priv_key);
            Assert.NotNull(keyPair.pub_key);
        }

        [Fact]
        public async Task CreateKeyPairAsync()
        {
            RustRsaKeyPair keyPair = await this._rustRsaWrapper.GetKeyPairAsync(4096);
            Assert.NotNull(keyPair.priv_key);
            Assert.NotNull(keyPair.pub_key);
        }
    }
}