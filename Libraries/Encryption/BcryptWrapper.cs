using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class BcryptWrapper
    {
        [DllImport("libperformant_encryption.so")]
        private static extern string bcrypt_hash(string passToHash);

        [DllImport("libperformant_encryption.so")]
        private static extern bool bcrypt_verify(string password, string hash);

        public string HashPassword(string passwordToHash)
        {
            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                return BCrypt.Net.BCrypt.HashPassword(passwordToHash);
            }
            else
            {
                return bcrypt_hash(passwordToHash);
            }
        }

        public async Task<string> HashPasswordAsync(string passwordToHash)
        {
            return await Task.Run(() =>
            {
                if (Environment.OSVersion.VersionString.Contains("Windows"))
                {
                    return BCrypt.Net.BCrypt.HashPassword(passwordToHash);
                }
                else
                {
                    return bcrypt_hash(passwordToHash);
                }
            });
        }
        public async Task<bool> Verify(string hashedPassword, string unhashed)
        {

            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                return BCrypt.Net.BCrypt.Verify(unhashed, hashedPassword);
            }
            else
            {
                return bcrypt_verify(unhashed, hashedPassword);
            }
        }
        public async Task<bool> VerifyAsync(string hashedPassword, string unhashed)
        {
            return await Task.Run(() =>
            {
                if (Environment.OSVersion.VersionString.Contains("Windows"))
                {
                    return BCrypt.Net.BCrypt.Verify(unhashed, hashedPassword);
                }
                else
                {
                    return bcrypt_verify(unhashed, hashedPassword);
                }
            });
        }
    }
}