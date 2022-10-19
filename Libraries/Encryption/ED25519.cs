using NSec.Cryptography;
using System.Threading.Tasks;

namespace Encryption
{
    public class ED25519
    {
        private readonly SignatureAlgorithm _signatureAlgo;

        public ED25519()
        {
            this._signatureAlgo = SignatureAlgorithm.Ed25519;
        }
        public async Task<Key> CreateKeyPair()
        {
            return await Task.Run(() =>
            {
                return Key.Create(this._signatureAlgo);
            });
        }
    }
}
