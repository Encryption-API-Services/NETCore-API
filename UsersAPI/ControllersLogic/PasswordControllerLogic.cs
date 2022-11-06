﻿using Common;
using DataLayer.Mongo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Validation.CreditCard;
using Encryption;
using DataLayer.Mongo.Repositories;
using MongoDB.Bson;
using Validation.UserRegistration;
using Models.UserAuthentication;

namespace UsersAPI.ControllersLogic
{
    public class PasswordControllerLogic : IPasswordControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IHashedPasswordRepository _hashedPasswordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        public PasswordControllerLogic(
            IMethodBenchmarkRepository methodBenchmarkRepository, 
            IHashedPasswordRepository hashedPasswordRepository, 
            IUserRepository userRepository,
            IForgotPasswordRepository forgotPasswordRepository
            )
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._hashedPasswordRepository = hashedPasswordRepository;
            this._userRepository = userRepository;
            this._forgotPasswordRepository = forgotPasswordRepository;
        }

        #region BcryptEncryprt
        public async Task<IActionResult> BcryptEncryptPassword([FromBody] BCryptEncryptModel body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Password))
                {
                    BcryptWrapper bcrypt = new BcryptWrapper();
                    string hashedPassword = await bcrypt.HashPasswordAsync(body.Password);
                    string id = ObjectId.GenerateNewId().ToString();
                    HashedPassword newPassword = new HashedPassword()
                    {
                        Id = id,
                        Password = hashedPassword,
                        CreateDate = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };
                    await this._hashedPasswordRepository.InsertOneHasedPassword(newPassword);
                    result = new OkObjectResult(new { hashedPassword = hashedPassword, ID = id });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region BcryptVerify
        public async Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body, HttpContext context)
        {
            IActionResult result = null;
            HashedPassword newPassword = await this._hashedPasswordRepository.GetOneHashedPassword(body.ID);
            if (newPassword != null)
            {
                BcryptWrapper wrapper = new BcryptWrapper();
                bool valid = await wrapper.Verify(newPassword.Password, body.Password);
                result = new OkObjectResult(new { IsValid = valid });
            }
            else
            {
                result = new BadRequestObjectResult(new { });
            }
            return result;
        }
        #endregion

        #region ForgotPassword
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequest body, HttpContext context)
        {
            IActionResult result = null;
            RegisterUserValidation validator = new RegisterUserValidation();
            if (validator.IsEmailValid(body.Email))
            {
                User databaseUser = await this._userRepository.GetUserByEmail(body.Email);
                ForgotPassword forgotPassword = new ForgotPassword()
                {
                    Token = Guid.NewGuid().ToString(),
                    HasBeenReset = false
                };
                await this._userRepository.UpdateForgotPassword(databaseUser.Id, forgotPassword);
            }
            return result;
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body, HttpContext context)
        {
            IActionResult result = null;
            User databaseUser = await this._userRepository.GetUserById(body.Id);
            if (databaseUser != null && body.Password.Equals(body.ConfirmPassword))
            { 

                BcryptWrapper wrapper = new BcryptWrapper();
                string hashedPassword = await wrapper.HashPasswordAsync(body.Password);
                // TODO: perform check that password hasn't been used 5 times.
                await this._userRepository.UpdatePassword(databaseUser.Id, hashedPassword);
                await this._forgotPasswordRepository.InsertForgotPasswordAttempt(databaseUser.Id, hashedPassword);
                result = new OkObjectResult(new { message = "You have successfully changed your password." });
            }
            return result;
        }
        #endregion
    }
}
