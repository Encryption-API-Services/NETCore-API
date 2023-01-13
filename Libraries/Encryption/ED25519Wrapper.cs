using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class ED25519Wrapper
    {

        [DllImport("performant_encryption.dll")]
        private static extern string get_ed25519_key_pair();
        [DllImport("performant_encryption.dll")]
        private static extern string sign_with_key_pair(string keyBytes, string dataToSign);
        [DllImport("performant_encryption.dll")]
        private static extern bool verify_with_key_pair(string keyBytes, string signature, string dataToVerify);

        public string GetKeyPair()
        {
            return get_ed25519_key_pair();
        }
        public async Task<string> GetKeyPairAsync()
        {
            return await Task.Run(() =>
            {
                return get_ed25519_key_pair();
            });
        }
        public string Sign(string keyBytes, string dataToSign)
        {
            if (string.IsNullOrEmpty(keyBytes))
            {
                throw new Exception("You need pass in the key bytes to sign data");
            }
            if (string.IsNullOrEmpty(dataToSign))
            {
                throw new Exception("You need to pass in data to sign, to sign data");
            }
            return sign_with_key_pair(keyBytes, dataToSign);
        }
        public async Task<string> SignAsync(string keyBytes, string dataToSign)
        {
            return await Task.Run(() =>
            {
                return Sign(keyBytes, dataToSign);
            });
        }

        public bool Verify(string keyBytes, string signature, string dataToVerify)
        {
            if (string.IsNullOrEmpty(keyBytes))
            {
                throw new Exception("You need pass in the key bytes to verify data");
            }
            if (string.IsNullOrEmpty(signature))
            {
                throw new Exception("You need to pass in the signature to verify data");
            }
            if (string.IsNullOrEmpty(dataToVerify))
            {
                throw new Exception("You need to pass in data to verify, to verify data");
            }

            return verify_with_key_pair(keyBytes, signature, dataToVerify);
        }
        public async Task<bool> VerifyAsync(string keyBytes, string signature, string dataToVerify)
        {
            return await Task.Run(() =>
            {
                return Verify(keyBytes, signature, dataToVerify);
            });
        }
    }
}