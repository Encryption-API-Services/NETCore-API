using Scrypt;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class SCryptWrapper
    {
        [DllImport("performant_encryption.dll")]
        private static extern string scrypt_hash(string passToHash);

        [DllImport("performant_encryption.dll")]
        private static extern bool scrypt_verify(string password, string hash);
        public string HashPassword(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("Please provide a password to hash");
            }
            return scrypt_hash(passToHash);
        }
        public async Task<string> HashPasswordAsync(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("Please provide a password to hash");
            }

            return await Task.Run(() =>
            {
                return scrypt_hash(passToHash);
            });
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }
            return scrypt_verify(password, hash);
        }

        public async Task<bool> VerifyPasswordAsync(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }

            return await Task.Run(() =>
            {
                return scrypt_verify(password, hash);
            });
        }
    }
}