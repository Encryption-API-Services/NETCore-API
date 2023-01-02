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
using System.Reflection;
using static Encryption.AESWrapper;

namespace API.ControllersLogic
{
    public class EncryptionControllerLogic : IEncryptionControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public EncryptionControllerLogic(
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IEASExceptionRepository exceptionRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._exceptionRepository = exceptionRepository;
        }
        #region DecryptAES
        public async Task<IActionResult> DecryptAES(DecryptAESRequest body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.DataToDecrypt) && !string.IsNullOrEmpty(body.Key) && !string.IsNullOrEmpty(body.NonceKey))
                {
                    AESWrapper aes = new AESWrapper();
                    string decrypted = await aes.DecryptPerformantAsync(body.NonceKey, body.Key, body.DataToDecrypt);
                    result = new OkObjectResult(new { Decrypted = decrypted });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
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
                if (!string.IsNullOrEmpty(body.DataToEncrypt) && !string.IsNullOrEmpty(body.NonceKey))
                {
                    AESWrapper aes = new AESWrapper();
                    AesEncrypt encrypted = await aes.EncryptPerformantAsync(body.NonceKey, body.DataToEncrypt);
                    result = new OkObjectResult(new { Nonce = body.NonceKey, Key = encrypted.key, Encrypted = encrypted.ciphertext });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
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
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
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
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}