using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class ED25519Wrapper
    {
        public struct Ed25519KeyPair
        {
            public string private_key { get; set; }
            public string public_key { get; set; }
        }

        [DllImport("performant_encryption.dll")]
        private static extern Ed25519KeyPair get_ed25519_key_pair();

        public Ed25519KeyPair GetKeyPair()
        {
            return get_ed25519_key_pair();
        }

        public async Task<Ed25519KeyPair> GetKeyPairAsync()
        {
            return await Task.Run(() =>
            {
                return get_ed25519_key_pair();
            });
        }
    }
}