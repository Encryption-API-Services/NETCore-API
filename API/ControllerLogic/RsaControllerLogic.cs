using Common;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using static Encryption.RustRSAWrapper;

namespace API.ControllerLogic
{
    public class RsaControllerLogic : IRsaControllerLogic
    {
        private readonly IEASExceptionRepository _exceptionRepository;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        public RsaControllerLogic(
            IEASExceptionRepository exceptionRepository,
            IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._exceptionRepository = exceptionRepository;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }

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
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}