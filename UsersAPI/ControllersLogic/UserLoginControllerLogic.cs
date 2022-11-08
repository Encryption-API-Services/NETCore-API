﻿using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public class UserLoginControllerLogic : IUserLoginControllerLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IFailedLoginAttemptRepository _failedLoginAttemptRepository;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;

        public UserLoginControllerLogic(
            IUserRepository userRepository, 
            IFailedLoginAttemptRepository failedLoginAttemptRepository,
            IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._userRepository = userRepository;
            this._failedLoginAttemptRepository = failedLoginAttemptRepository;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }

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
                    JWT jwtWrapper = new JWT();
                    string signingKey = handler.Claims.First(x => x.Type == "private-key").Value;
                    string userId = jwtWrapper.GetUserIdFromToken(token);
                    RSACryptoServiceProvider rsaProvdier = new RSACryptoServiceProvider(4096);
                    rsaProvdier.FromXmlString(signingKey);
                    RSAParameters parameters = rsaProvdier.ExportParameters(true);
                    if (!await jwtWrapper.ValidateSecurityToken(token, parameters))
                    {
                        RSAProviderWrapper rsa4096 = new RSAProviderWrapper(4096);
                        User activeUser = await this._userRepository.GetUserById(userId);
                        string newToken = new JWT().GenerateSecurityToken(activeUser.Id, rsa4096.rsaParams, rsa4096.privateKey);
                        JwtToken jwtToken = new JwtToken()
                        {
                            Token = newToken,
                            PrivateKey = rsa4096.privateKey,
                            PublicKey = rsa4096.publicKey
                        };
                        await this._userRepository.UpdateUsersJwtToken(activeUser, jwtToken);
                        result = new OkObjectResult(new { token = newToken });
                    }
                }
                else
                {
                    // if token is still valid just send the same token back.
                    result = new OkObjectResult(new { token = token });
                }

            }
            catch (Exception ex)
            {
                // TODO: give error message
                result = new BadRequestObjectResult(new { });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        #endregion

        #region LoginUser
        public async Task<IActionResult> LoginUser(LoginUser body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                User activeUser = await this._userRepository.GetUserByEmail(body.Email);
                if (activeUser != null && activeUser.LockedOut.IsLockedOut == false && activeUser.IsActive == true)
                {
                    BcryptWrapper wrapper = new BcryptWrapper();
                    if (await wrapper.Verify(activeUser.Password, body.Password))
                    {
                        // TODO: abstract the RSAParameters to another class that contains the already exported public and private keys in XML to be save in database.
                        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                        RSAParameters rsaParams = RSAalg.ExportParameters(true);
                        string privateKey = RSAalg.ToXmlString(true);
                        string token = new JWT().GenerateSecurityToken(activeUser.Id, rsaParams, privateKey);
                        JwtToken jwtToken = new JwtToken()
                        {
                            Token = token,
                            PrivateKey = RSAalg.ToXmlString(true),
                            PublicKey = RSAalg.ToXmlString(false)
                        };
                        await this._userRepository.UpdateUsersJwtToken(activeUser, jwtToken);
                        result = new OkObjectResult(new { message = "You have successfully signed in.", token = token });
                    }
                    else
                    {
                        FailedLoginAttempt attempt = new FailedLoginAttempt()
                        {
                            Password = body.Password,
                            CreateDate = DateTime.UtcNow,
                            LastModifed = DateTime.UtcNow,
                            UserAccount = activeUser.Id
                        };
                        await this._failedLoginAttemptRepository.InsertFailedLoginAttempt(attempt);
                        List<FailedLoginAttempt> lastTwelveHourAttempts = await this._failedLoginAttemptRepository.GetFailedLoginAttemptsLastTweleveHours(activeUser.Id);
                        if (lastTwelveHourAttempts.Count >= 5)
                        {
                            await this._userRepository.LockoutUser(activeUser.Id);
                        }
                        result = new BadRequestObjectResult(new { error = "You entered an invalid password." });
                    }
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "This user account has been locked out due to many failed login attempts." });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "Something went wrong on our end. Please try again." });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region UnlockUser
        public async Task<IActionResult> UnlockUser(UnlockUser body, HttpContext context)
        {
            // TODO: benchmarket logger
            IActionResult result = null;
            if (!string.IsNullOrEmpty(body.Id))
            {
                await this._userRepository.UnlockUser(body.Id);
            }
            return result;
        }
        #endregion
    }
}
