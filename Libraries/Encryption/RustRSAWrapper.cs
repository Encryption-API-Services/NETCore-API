﻿using System;
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
        public struct RsaSignResult
        {
            public string signature;
            public string public_key;
        }

        [DllImport("performant_encryption.dll")]
        private static extern RustRsaKeyPair get_key_pair(int key_size);
        [DllImport("performant_encryption.dll")]
        private static extern string rsa_encrypt(string publicKey, string dataToEncrypt);

        [DllImport("performant_encryption.dll")]
        private static extern string rsa_decrypt(string privateKey, string dataToDecrypt);
        [DllImport("performant_encryption.dll")]
        private static extern RsaSignResult rsa_sign(string dataToSign, int keySize);

        public RsaSignResult RsaSign(string dataToSign, int keySize)
        {
            if (string.IsNullOrEmpty(dataToSign))
            {
                throw new Exception("You must provide data to sign with RSA");
            }
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("You must provide a valid key bit size to sign with RSA");
            }
            return rsa_sign(dataToSign, keySize);
        }

        public async Task<RsaSignResult> RsaSignAsync(string dataToSign, int keySize)
        {
            if (string.IsNullOrEmpty(dataToSign))
            {
                throw new Exception("You must provide data to sign with RSA");
            }
            if (keySize != 1024 && keySize != 2048 && keySize != 4096)
            {
                throw new Exception("You must provide a valid key bit size to sign with RSA");
            }
            return await Task.Run(() =>
            {
                return rsa_sign(dataToSign, keySize);

            });
        }
        public string RsaDecrypt(string privateKey, string dataToDecrypt)
        {
            if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(dataToDecrypt))
            {
                throw new Exception("You need to provide a private key and data to decrypt to use RsaCrypt");
            }
            return rsa_decrypt(privateKey, dataToDecrypt);
        }
        public async Task<string> RsaDecryptAsync(string privateKey, string dataToDecrypt)
        {
            if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(dataToDecrypt))
            {
                throw new Exception("You need to provide a private key and data to decrypt to use RsaCrypt");
            }
            return await Task.Run(() =>
            {
                return rsa_decrypt(privateKey, dataToDecrypt);
            });
        }
        public string RsaEncrypt(string publicKey, string dataToEncrypt)
        {
            if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(dataToEncrypt))
            {
                throw new Exception("You need to provide a public key and data to encrypt to use RsaEncrypt");
            }
            return rsa_encrypt(publicKey, dataToEncrypt);
        }
        public async Task<string> RsaEncryptAsync(string publicKey, string dataToEncrypt)
        {
            if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(dataToEncrypt))
            {
                throw new Exception("You need to provide a public key and data to encrypt to use RsaEncrypt");
            }
            return await Task.Run(() =>
            {
                return rsa_encrypt(publicKey, dataToEncrypt);
            });
        }

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
