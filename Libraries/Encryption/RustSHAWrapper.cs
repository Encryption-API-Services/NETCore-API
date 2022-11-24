using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class RustSHAWrapper
    {
        [DllImport("performant_encryption.dll")]
        private static extern string sha512_hash_password(string password);
        [DllImport("performant_encryption.dll")]
        private static extern bool sha512_verify_password(string password, string hashedPassword);

        public string HashString(string stringTohash)
        {
            if (string.IsNullOrEmpty(stringTohash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return sha512_hash_password(stringTohash);
        }

        public async Task<string> HashPasswordAsync(string stringTohash)
        {
            if (string.IsNullOrEmpty(stringTohash))
            {
                throw new Exception("Please provide a string to hash");
            }
            return await Task.Run(() =>
            {
                return sha512_hash_password(stringTohash);
            });
        }
    }
}
