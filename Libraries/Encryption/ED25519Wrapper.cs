using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class ED25519Wrapper
    { 

        [DllImport("performant_encryption.dll")]
        private static extern string get_ed25519_key_pair();

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
    }
}