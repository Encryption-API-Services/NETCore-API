using System.Security.Cryptography;

namespace Encryption
{
    public sealed class RSAProviderWrapper
    {
        public RSACryptoServiceProvider _provider { get; set; }
        public string _privateKey { get; set; }
        public string _publicKey { get; set; }
        public RSAProviderWrapper(int? keySize)
        {
            switch (keySize)
            {
                case 4096:
                    this._provider = new RSACryptoServiceProvider(4096);
                    return;
                case 2048:
                    this._provider = new RSACryptoServiceProvider(2048);
                    return;
                case 1024:
                    this._provider = new RSACryptoServiceProvider(1024);
                    return;
                default:
                    this._provider = new RSACryptoServiceProvider(4096);
                    return;
            }
            this.SetKeyPair();
        }
        private void SetKeyPair()
        {
            this._privateKey = this._provider.ToXmlString(true);
            this._publicKey = this._provider.ToXmlString(false);
        }
    }
}