using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class RustSHAWrapper
    {
        [DllImport("performant_encryption.dll")]
        private static extern string sha512(string password);

        public string HashString(string stringTohash)
        {
            if (string.IsNullOrEmpty(stringTohash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return sha512(stringTohash);
        }

        public async Task<string> HashPasswordAsync(string stringTohash)
        {
            if (string.IsNullOrEmpty(stringTohash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return await Task.Run(() =>
            {
                return sha512(stringTohash);
            });
        }
    }
}