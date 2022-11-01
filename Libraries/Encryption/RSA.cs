using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class RSA
    {
        public RSACryptoServiceProvider GetNewRsaProvider(int? keySize)
        {
            switch (keySize)
            {
                case 4096:
                    return new RSACryptoServiceProvider(4096);
                case 2048:
                    return new RSACryptoServiceProvider(2048);
                case 1024:
                    return new RSACryptoServiceProvider(1024);
                default:
                    return new RSACryptoServiceProvider();
            }
        }
    }
}
