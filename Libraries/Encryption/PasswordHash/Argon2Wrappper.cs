using System;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption.PasswordHash
{
    public class Argon2Wrappper
    {
        [DllImport("performant_encryption.dll")]
        private static extern IntPtr argon2_hash(string passToHash);
        [DllImport("performant_encryption.dll")]
        private static extern bool argon2_verify(string hashedPassword, string passToVerify);
        [DllImport("performant_encryption.dll")]
        public static extern IntPtr free_argon2_string();

        public IntPtr HashPassword(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("You must provide a password to hash using argon2");
            }
            return argon2_hash(passToHash);
        }

        public async Task<IntPtr> HashPasswordAsync(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("You must provide a password to hash using argon2");
            }
            return await Task.Run(() =>
            {
                return argon2_hash(passToHash);
            });
        }

        public bool VerifyPassword(string hashedPasswrod, string password)
        {
            if (string.IsNullOrEmpty(hashedPasswrod) || string.IsNullOrEmpty(password))
            {
                throw new Exception("You must provide a hashed password and password to verify with argon2");
            }
            return argon2_verify(hashedPasswrod, password);
        }
        public async Task<bool> VerifyPasswordAsync(string hashedPasswrod, string password)
        {
            if (string.IsNullOrEmpty(hashedPasswrod) || string.IsNullOrEmpty(password))
            {
                throw new Exception("You must provide a hashed password and password to verify with argon2");
            }
            return await Task.Run(() =>
            {
                return argon2_verify(hashedPasswrod, password);
            });
        }
    }
}
