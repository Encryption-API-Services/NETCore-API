using Scrypt;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class SCryptWrapper
    {
        [DllImport("libperformant_encryption.so")]
        private static extern string scrypt_hash(string passToHash);

        [DllImport("libperformant_encryption.so")]
        private static extern bool scrypt_verify(string password, string hash);
        public string HashPassword(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("Please provide a password to hash");
            }

            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                ScryptEncoder scrypt = new ScryptEncoder();
                return scrypt.Encode(passToHash);
            } 
            else
            {
                return scrypt_hash(passToHash);
            }
        }
        public async Task<string> HashPasswordAsync(string passToHash)
        {
            if (string.IsNullOrEmpty(passToHash))
            {
                throw new Exception("Please provide a password to hash");
            }

            return await Task.Run(() =>
            {
                if (Environment.OSVersion.VersionString.Contains("Windows"))
                {
                    ScryptEncoder scrypt = new ScryptEncoder();
                    return scrypt.Encode(passToHash);
                }
                else
                {
                    return scrypt_hash(passToHash);
                }
            });
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }

            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                ScryptEncoder scrypt = new ScryptEncoder();
                return scrypt.Compare(password, hash);
            }
            else
            {
                return scrypt_verify(password, hash);
            }
        }

        public async Task<bool> VerifyPasswordAsync(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
            {
                throw new Exception("Please provide a password and a hash to verify");
            }

            return await Task.Run(() =>
            {
                if (Environment.OSVersion.VersionString.Contains("Windows"))
                {
                    ScryptEncoder scrypt = new ScryptEncoder();
                    return scrypt.Compare(password, hash);
                }
                else
                {
                    return scrypt_verify(password, hash);
                }
            });
        }
    }
}