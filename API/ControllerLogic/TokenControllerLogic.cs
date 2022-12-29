﻿using Common;
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
                    ECDSAWrapper ecdsa = new ECDSAWrapper("ES521");
                    string token = new JWT().GenerateECCToken(activeUser.Id, activeUser.IsAdmin, ecdsa);
                    result = new OkObjectResult(new { token = token });
                }
                return result;

            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end"});
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
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
                    ECDSAWrapper ecdsa = new ECDSAWrapper("ES521");
                    ecdsa.ImportFromPublicBase64String(publicKey);
                    JWT jwtWrapper = new JWT();
                    if (!await jwtWrapper.ValidateECCToken(token, ecdsa.ECDKey))
                    {
                        string userId = jwtWrapper.GetUserIdFromToken(token);
                        bool isAdmin = bool.Parse(handler.Claims.First(x => x.Type == "IsAdmin").Value);
                        ECDSAWrapper newEcdsa = new ECDSAWrapper("ES521");
                        string newToken = new JWT().GenerateECCToken(userId, isAdmin, newEcdsa);
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