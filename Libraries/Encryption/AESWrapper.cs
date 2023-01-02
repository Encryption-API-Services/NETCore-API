using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Encryption
{
    /// <summary>
    /// Usage example: 
    /// 
    /// using (AesManaged myAes = new AesManaged())
    //{
    // Encrypt the string to an array of bytes.
    //byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

    // Decrypt the bytes to a string.
    // string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

    //Display the original data and the decrypted data.
    //Console.WriteLine("Original:   {0}", original);
    //            Console.WriteLine("Round Trip: {0}", roundtrip);
    // }
    /// </summary>
    public class AESWrapper
    {
        public struct AesEncrypt
        {
            public string key { get; set; }
            public string ciphertext { get; set; }
        }

        [DllImport("performant_encryption.dll")]
        private static extern AesEncrypt aes256_encrypt_string(string nonceKey, string dataToEncrypt);
        [DllImport("performant_encryption.dll")]
        private static extern string aes256_decrypt_string(string nonceKey, string key, string dataToDecrypt);

        public AesEncrypt EncryptPerformant(string nonceKey, string toEncrypt)
        {
            if (!string.IsNullOrEmpty(nonceKey) && !string.IsNullOrEmpty(toEncrypt))
            {
                return aes256_encrypt_string(nonceKey, toEncrypt);
            }
            else
            {
                throw new Exception("You need to pass in a valid key and text string to encrypt");
            }
        }

        public async Task<AesEncrypt> EncryptPerformantAsync(string nonceKey, string toEncrypt)
        {
            if (!string.IsNullOrEmpty(nonceKey) && !string.IsNullOrEmpty(toEncrypt))
            {
                return await Task.Run(() =>
                {
                    return aes256_encrypt_string(nonceKey, toEncrypt);
                });
            }
            else
            {
                throw new Exception("You need to pass in a valid key and text string to encrypt");
            }
        }

        public string DecryptPerformant(string nonceKey, string key, string toDecrypt)
        {
            if (!string.IsNullOrEmpty(nonceKey) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(toDecrypt))
            {
                return aes256_decrypt_string(nonceKey, key, toDecrypt);
            }
            else
            {
                throw new Exception("You need to provide a nonce key, key, and data to decrypt to use AES-GCM");
            }
        }
        public async Task<string> DecryptPerformantAsync(string nonceKey, string key, string toDecrypt)
        {
            if (!string.IsNullOrEmpty(nonceKey) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(toDecrypt))
            {
                return await Task.Run(() =>
                {
                    return aes256_decrypt_string(nonceKey, key, toDecrypt);
                });
            }
            else
            {
                throw new Exception("You need to provide a nonce key, key, and data to decrypt to use AES-GCM");
            }
        }


        public byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }

        public byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}