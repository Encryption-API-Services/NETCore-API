using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Encryption.Tests
{
    public class MD5WrapperTests
    {
        private MD5Wrapper _md5Wrapper { get; set; }
        public MD5WrapperTests()
        {
            _md5Wrapper = new MD5Wrapper();
        }

        [Fact]
        public void CreateHash()
        {
            byte[] asciiBytpes = Encoding.ASCII.GetBytes("string to test MD5");
            string md5 = _md5Wrapper.CreateMD5(asciiBytpes);
            string md523 = _md5Wrapper.CreateMD5(asciiBytpes);
            Assert.IsType<string>(md5);
            Assert.IsType<string>(md523);
            Assert.Equal(md5, md523);
        }

        [Fact]
        public async Task CreateHashAsync()
        {
            byte[] asciiBytpes = Encoding.ASCII.GetBytes("string to test MD5");
            Task<string> md5 = _md5Wrapper.CreateMD5Async(asciiBytpes);
            Task<string> md523 = _md5Wrapper.CreateMD5Async(asciiBytpes);
            await Task.WhenAll(md5, md523);
            Assert.IsType<string>(md5.Result);
            Assert.IsType<string>(md523.Result);
            Assert.Equal(md5.Result, md523.Result);
        }
    }
}
