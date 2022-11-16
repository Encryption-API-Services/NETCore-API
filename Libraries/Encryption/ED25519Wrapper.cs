using NSec.Cryptography;
using System.Threading.Tasks;

namespace Encryption
{
    public class ED25519Wrapper
    {
        private readonly SignatureAlgorithm _signatureAlgo;
        private Key key { get; set; }

        public ED25519()
        {
            this._signatureAlgo = SignatureAlgorithm.Ed25519;
            this.CreateKeyPair().GetAwaiter().GetResult();
        }
        private async Task CreateKeyPair()
        {
            this.key = await Task.Run(() =>
            {
                return Key.Create(this._signatureAlgo);
            });
        }

        /// <summary>
        /// Should take in UTF8 bytes.
        /// </summary>
        /// <param name="dataToSign"></param>
        public async Task<byte[]> SignDataAsync(byte[] dataToSign)
        {
            return await Task.Run(() =>
            {
                return this._signatureAlgo.Sign(this.key, dataToSign);
            });
        }
    }
}
