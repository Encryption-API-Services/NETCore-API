using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Encryption;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Reflection;
using System.Runtime.InteropServices;
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
                    RustRSAWrapper.free_rsa_decrypt_string();
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
                    string privateKey = Marshal.PtrToStringUTF8(keyPair.priv_key);
                    string publicKey = Marshal.PtrToStringUTF8(keyPair.pub_key);
                    RustRSAWrapper.free_rsa_key_pair();
                    string encrypted = await rsaWrapper.RsaEncryptAsync(publicKey, body.dataToEncrypt);
                    RsaEncryption rsaEncryption = new RsaEncryption()
                    {
                        UserId = context.Items["UserID"].ToString(),
                        PublicKey = publicKey,
                        PrivateKey = privateKey,
                        CreatedDate = DateTime.UtcNow
                    };
                    await this._rsaEncryptionRepository.InsertNewEncryption(rsaEncryption);
                    result = new OkObjectResult(new { PublicKey = publicKey, encryptedData = encrypted });
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
                    string publicKey = Marshal.PtrToStringUTF8(keyPair.pub_key);
                    string privateKey = Marshal.PtrToStringUTF8(keyPair.priv_key);
                    RustRSAWrapper.free_rsa_key_pair();
                    result = new OkObjectResult(new { PublicKey = publicKey, PrivateKey = privateKey });
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

        #region SignWithoutKey
        public async Task<IActionResult> SignWithoutKey(HttpContext context, RsaSignWithoutKeyRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(body.dataToSign))
                {
                    result = new BadRequestObjectResult(new { message = "You must provide data to sign with RSA" });
                }
                else if (body.keySize != 1024 && body.keySize != 2048 && body.keySize != 4096)
                {
                    result = new BadRequestObjectResult(new { message = "You must provide a valid RSA key bit size to sign your data" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    RsaSignResult rsaSignResult = await rsaWrapper.RsaSignAsync(body.dataToSign, body.keySize);
                    RustRSAWrapper.free_rsa_sign_strings();
                    result = new OkObjectResult(new { PublicKey = rsaSignResult.public_key, Signature = rsaSignResult.signature });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { message = "There was an error on our end signing your data for you" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region Verify
        public async Task<IActionResult> Verify(HttpContext context, RsaVerifyRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(body.PublicKey))
                {
                    result = new BadRequestObjectResult(new { message = "You must provide a public key to verify" });
                }
                else if (string.IsNullOrEmpty(body.OriginalData))
                {
                    result = new BadRequestObjectResult(new { message = "You must provide the original data to verify its signature" });
                }
                else if (string.IsNullOrEmpty(body.Signature))
                {
                    result = new BadRequestObjectResult(new { message = "You must provide the RSA signature we computed or you to verify with RSA" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    bool isValid = await rsaWrapper.RsaVerifyAsync(body.PublicKey, body.OriginalData, body.Signature);
                    result = new OkObjectResult(new { IsValid = isValid });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { message = "There was an error on our end verifying your data for you, did you provide the appropriate unaltered data?" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
            #endregion
        }

        #region VerifyWithKey
        public async Task<IActionResult> VerifyWithKey(HttpContext context, RsaSignWithKeyRequest body)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (string.IsNullOrEmpty(body.PrivateKey))
                {
                    result = new BadRequestObjectResult(new { message = "You need to provide a private key to verify your data signature" });
                }
                else if (string.IsNullOrEmpty(body.DataToSign))
                {
                    result = new BadRequestObjectResult(new { message = "You need to provide the data to sign" });
                }
                else
                {
                    RustRSAWrapper rsaWrapper = new RustRSAWrapper();
                    string signature = await rsaWrapper.RsaSignWithKeyAsync(body.PrivateKey, body.DataToSign);
                    result = new OkObjectResult(new { Signature = signature });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { message = "There was an error on our end verifying your data for you, did you provide the appropriate unaltered data?" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}