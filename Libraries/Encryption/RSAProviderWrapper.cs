﻿using System.Security.Cryptography;

namespace Encryption
{
    public sealed class RSAProviderWrapper
    {
        public RSACryptoServiceProvider provider { get; set; }
        public string privateKey { get; set; }
        public string publicKey { get; set; }
        public RSAParameters rsaParams { get; set; }
        public RSAProviderWrapper(int? keySize)
        {
            switch (keySize)
            {
                case 4096:
                    this.provider = new RSACryptoServiceProvider(4096);
                    return;
                case 2048:
                    this.provider = new RSACryptoServiceProvider(2048);
                    return;
                case 1024:
                    this.provider = new RSACryptoServiceProvider(1024);
                    return;
                default:
                    this.provider = new RSACryptoServiceProvider(4096);
                    return;
            }
            this.SetKeyPair();
        }
        private void SetKeyPair()
        {
            this.privateKey = this.provider.ToXmlString(true);
            this.publicKey = this.provider.ToXmlString(false);
        }
    }
}