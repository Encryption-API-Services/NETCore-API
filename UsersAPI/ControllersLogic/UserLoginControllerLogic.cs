﻿using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public class UserLoginControllerLogic : IUserLoginControllerLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IFailedLoginAttemptRepository _failedLoginAttemptRepository;

        public UserLoginControllerLogic(IUserRepository userRepository, IFailedLoginAttemptRepository failedLoginAttemptRepository)
        {
            this._userRepository = userRepository;
            this._failedLoginAttemptRepository = failedLoginAttemptRepository;
        }

        public async Task<IActionResult> GetRefreshToken(HttpContext context)
        {
            // TOOD: benchmark logger.
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
                        // TODO: abstract the RSAParameters to another class that contains the already exported public and private keys in XML to be save in database.
                        RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                        RSAParameters rsaParams = RSAalg.ExportParameters(true);
                        string privateKey = RSAalg.ToXmlString(true);
                        User activeUser = await this._userRepository.GetUserById(userId);
                        string newToken = new JWT().GenerateSecurityToken(activeUser.Id, rsaParams, privateKey);
                        JwtToken jwtToken = new JwtToken()
                        {
                            Token = newToken,
                            PrivateKey = RSAalg.ToXmlString(true),
                            PublicKey = RSAalg.ToXmlString(false)
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
                result = new BadRequestObjectResult(new { });
            }
            return result;
        }

        public async Task<IActionResult> LoginUser(LoginUser body)
        {
            // TODO: Benchmark logger.
            IActionResult result = null;
            try
            {
                User activeUser = await this._userRepository.GetUserByEmail(body.Email);
                if (activeUser != null)
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
                        result = new OkObjectResult(new { token = token });
                    }
                    else
                    {
                        
                        result = new BadRequestObjectResult(new { error = "You entered an invalid password" });
                    }
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = ex.ToString() });
            }
            return result;
        }
    }
}
