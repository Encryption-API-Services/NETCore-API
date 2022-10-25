using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Encryption;
using System.Text;

namespace UsersAPI.ControllersLogic
{
    public class EncryptionControllerLogic : IEncryptionControllerLogic
    {
        public async Task<IActionResult> EncryptAES(EncryptAESRequest body)
        {
            IActionResult result = null;
            try
            {
                using (AesManaged aes = new AesManaged())
                {
                    byte[] key = aes.Key;
                    byte[] iv = aes.IV;
                    AESWrapper aesWrapper = new AESWrapper();
                    string encryptedString = Convert.ToBase64String(aesWrapper.Encrypt(body.DataToEncrypt, key, iv));
                    result = new OkObjectResult(new { data = encryptedString, key = Convert.ToBase64String(key), iv = Convert.ToBase64String(iv) });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestResult();
            }
            return result;
        }
    }
}
