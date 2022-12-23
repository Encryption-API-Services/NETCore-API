using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class BcryptWrapper
    {
        [DllImport("performant_encryption.dll")]
        private static extern string bcrypt_hash(string passToHash);

        [DllImport("performant_encryption.dll")]
        private static extern bool bcrypt_verify(string password, string hash);

        public string HashPassword(string passwordToHash)
        {
            return bcrypt_hash(passwordToHash);
        }

        public async Task<string> HashPasswordAsync(string passwordToHash)
        {
            return await Task.Run(() =>
            {
                return bcrypt_hash(passwordToHash);
            });
        }
        public async Task<bool> Verify(string hashedPassword, string unhashed)
        {
            return bcrypt_verify(unhashed, hashedPassword);
        }
        public async Task<bool> VerifyAsync(string hashedPassword, string unhashed)
        {
            return await Task.Run(() =>
            {
                return bcrypt_verify(unhashed, hashedPassword);
            });
        }
    }
}