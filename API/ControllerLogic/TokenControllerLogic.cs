using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Cryptography;

namespace API.ControllerLogic
{
    public class TokenControllerLogic : ITokenControllerLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public TokenControllerLogic(
            IUserRepository userRepository,
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IEASExceptionRepository exceptionRepository)
        {
            this._userRepository = userRepository;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._exceptionRepository = exceptionRepository;
        }
        #region GetToken
        public async Task<IActionResult> GetToken(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string apiKey = httpContext.Request.Headers["ApiKey"].ToString();
                if (string.IsNullOrEmpty(apiKey))
                {
                    result = new UnauthorizedObjectResult(new { error = "You did not set an ApiKey" });
                }
                User activeUser = await this._userRepository.GetUserByApiKey(apiKey);
                if (activeUser == null)
                {
                    result = new UnauthorizedObjectResult(new { error = "You entered an invalid ApiKey" });
                }
                else if (activeUser.IsActive == false)
                {
                    result = new UnauthorizedObjectResult(new { error = "This user account is no longer active" });
                }
                else
                {
                    RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                    RSAParameters rsaParams = RSAalg.ExportParameters(true);
                    string publicKey = RSAalg.ToXmlString(false);
                    string token = new JWT().GenerateSecurityToken(activeUser.Id, rsaParams, publicKey, activeUser.IsAdmin);
                    result = new OkObjectResult(new { token = token });
                }
                return result;

            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end"});
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
            return result;
        }
        #endregion

        #region GetRefreshToken
        public async Task<IActionResult> GetRefreshToken(HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                // get current token
                string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    string publicKey = handler.Claims.First(x => x.Type == "public-key").Value;
                    RSACryptoServiceProvider rsaProvdier = new RSACryptoServiceProvider(4096);
                    rsaProvdier.FromXmlString(publicKey);
                    RSAParameters parameters = rsaProvdier.ExportParameters(false);
                    JWT jwtWrapper = new JWT();
                    if (!await jwtWrapper.ValidateSecurityToken(token, parameters))
                    {
                        RSAProviderWrapper rsa4096 = new RSAProviderWrapper(4096);
                        string userId = jwtWrapper.GetUserIdFromToken(token);
                        bool isAdmin = bool.Parse(handler.Claims.First(x => x.Type == "IsAdmin").Value);
                        rsa4096.SetPrivateParams();
                        string newToken = new JWT().GenerateSecurityToken(userId, rsa4096.rsaParams, rsa4096.publicKey, isAdmin);
                        result = new OkObjectResult(new { token = newToken });
                    }
                    else
                    {
                        // if token is still valid just send the same token back.
                        result = new OkObjectResult(new { token = token });
                    }
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                // TODO: give error message
                result = new BadRequestObjectResult(new { });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}