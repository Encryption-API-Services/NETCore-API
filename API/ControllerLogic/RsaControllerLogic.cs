using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Reflection;
using static Encryption.RustRSAWrapper;

namespace API.ControllerLogic
{
    public class RsaControllerLogic : IRsaControllerLogic
    {
        private readonly IEASExceptionRepository _exceptionRepository;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IRsaEncryptionRepository _rsaEncryptionRepository;
        public RsaControllerLogic(
            IEASExceptionRepository exceptionRepository,
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IRsaEncryptionRepository rsaEncryptionRepository)
        {
            this._exceptionRepository = exceptionRepository;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._rsaEncryptionRepository = rsaEncryptionRepository;
        }

        #region Decrypt
        public async Task<IActionResult> Decrypt(HttpContext context, RsaDecryptRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(body.PublicKey) || string.IsNullOrEmpty(body.DataToDecrypt))
                {
                    result = new BadRequestObjectResult(new { message = "You need to provide a public key and data to decrypt" });
                }
                else
                {
                    string userId = context.Items["UserID"].ToString();
                    RsaEncryption rsaEncryption = await this._rsaEncryptionRepository.GetEncryptionByIdAndPublicKey(userId, body.PublicKey);
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    string decryptedData = await rsaWrapper.RsaDecryptAsync(rsaEncryption.PrivateKey, body.DataToDecrypt);
                    result = new OkObjectResult(new { decryptedData = decryptedData });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { message = " Something went wrong on our end while decrypting your data" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region EncryptWithoutPublic
        public async Task<IActionResult> EncryptWithoutPublic(HttpContext context, EncryptWithoutPublicRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (body.keySize != 1024 && body.keySize != 2048 && body.keySize != 4096)
                {
                    result = new BadRequestObjectResult(new { message = "You must provide a valid key size to encrypt your data" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    RustRsaKeyPair keyPair = await rsaWrapper.GetKeyPairAsync(body.keySize);
                    string encrypted = await rsaWrapper.RsaEncryptAsync(keyPair.pub_key, body.dataToEncrypt);
                    RsaEncryption rsaEncryption = new RsaEncryption()
                    {
                        UserId = context.Items["UserID"].ToString(),
                        PublicKey = keyPair.pub_key,
                        PrivateKey = keyPair.priv_key,
                        CreatedDate = DateTime.UtcNow
                    };
                    await this._rsaEncryptionRepository.InsertNewEncryption(rsaEncryption);
                    result = new OkObjectResult(new { PublicKey = keyPair.pub_key, encryptedData = encrypted });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        #endregion

        #region EncryptWithPublic
        public async Task<IActionResult> EncryptWithPublic(HttpContext context, EncryptWithPublicRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(body.PublicKey) && string.IsNullOrEmpty(body.DataToEncrypt))
                {
                    result = new BadRequestObjectResult(new { message = "You must provide a public key we generated for you and data to encrypt" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    string encrypted = await rsaWrapper.RsaEncryptAsync(body.PublicKey, body.DataToEncrypt);
                    result = new OkObjectResult(new { encryptedData = encrypted });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region GetKeyPair
        public async Task<IActionResult> GetKeyPair(HttpContext context, int keySize)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (keySize != 1024 && keySize != 2048 && keySize != 4096)
                {
                    result = new BadRequestObjectResult(new { message = "You need to specify a valid key size to generate RSA keys" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    RustRsaKeyPair keyPair = await rsaWrapper.GetKeyPairAsync(keySize);
                    result = new OkObjectResult(new { PublicKey = keyPair.pub_key, PrivateKey = keyPair.priv_key });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { message = "There was an error on our end generating a key pair for you" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}