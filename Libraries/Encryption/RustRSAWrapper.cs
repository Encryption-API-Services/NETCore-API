using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class RustRSAWrapper
    {
        public struct RustRsaKeyPair
        {
            public string pub_key;
            public string priv_key;
        }

        [DllImport("performant_encryption.dll")]
        private static extern RustRsaKeyPair get_key_pair(int key_size);

        public RustRsaKeyPair GetKeyPair(int keySize)
        {
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("Please pass in a valid key size.");
            }
            return get_key_pair(keySize);
        }
        public async Task<RustRsaKeyPair> GetKeyPairAsync(int keySize)
        {
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("Please pass in a valid key size.");
            }
            return await Task.Run(() =>
            {
                return get_key_pair(keySize);
            });
        }
    }
}
