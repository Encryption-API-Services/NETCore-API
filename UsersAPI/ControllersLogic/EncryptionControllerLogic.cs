using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Encryption;
using Common;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using System.Text;
using MongoDB.Bson.Serialization.IdGenerators;

namespace UsersAPI.ControllersLogic
{
    public class EncryptionControllerLogic : IEncryptionControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        public EncryptionControllerLogic(IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }
        #region DecryptAES
        public async Task<IActionResult> DecryptAES(DecryptAESRequest body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Data) && !string.IsNullOrEmpty(body.Key) && !string.IsNullOrEmpty(body.IV))
                {
                    using (AesManaged aes = new AesManaged())
                    {
                        AESWrapper aesWrapper = new AESWrapper();
                        byte[] cipherText = Convert.FromBase64String(body.Data);
                        byte[] key = Convert.FromBase64String(body.Key);
                        byte[] iv = Convert.FromBase64String(body.IV);
                        string decryptedString = aesWrapper.DecryptStringFromBytes_Aes(cipherText, key, iv);
                        result = new OkObjectResult(new { data = decryptedString });
                    }
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestResult();
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region EncryptAES
        public async Task<IActionResult> EncryptAES(EncryptAESRequest body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.DataToEncrypt))
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
            }
            catch (Exception ex)
            {
                result = new BadRequestResult();
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion


        #region EncryptSHA
        public async Task<IActionResult> EncryptSHA(EncryptSHARequest body, HttpContext httpContext, SHATypes type)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.DataToEncrypt))
                {
                    using (HashAlgorithm sha = new ManagedSHAFactory().Get(type))
                    {
                        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(body.DataToEncrypt));
                        var sb = new StringBuilder(hash.Length * 2);
                        foreach (byte b in hash)
                        {
                            sb.Append(b.ToString("x2"));
                        }
                        string hashToReturn = sb.ToString();
                        result = new OkObjectResult(new { hash = hashToReturn });

                    }
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestResult();
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region HashMD5
        public async Task<IActionResult> HashMD5(MD5Request body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.DataToHash))
                {
                    //convert body string to ascii bytes
                    byte[] asciiByes = Encoding.ASCII.GetBytes(body.DataToHash);
                    string hash = await new MD5Wrapper().CreateMD5Async(asciiByes);
                    result = new OkObjectResult(new { hash = hash });
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "You need to specific data to hash" });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}