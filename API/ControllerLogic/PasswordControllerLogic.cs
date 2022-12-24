using Common;
using DataLayer.Mongo.Entities;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using Encryption;
using DataLayer.Mongo.Repositories;
using MongoDB.Bson;
using Validation.UserRegistration;
using Models.UserAuthentication;
using User = DataLayer.Mongo.Entities.User;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Encryption.PasswordHash;

namespace API.ControllersLogic
{
    public class PasswordControllerLogic : IPasswordControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IHashedPasswordRepository _hashedPasswordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public PasswordControllerLogic(
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IHashedPasswordRepository hashedPasswordRepository,
            IUserRepository userRepository,
            IForgotPasswordRepository forgotPasswordRepository,
            IEASExceptionRepository exceptionRepository
            )
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._hashedPasswordRepository = hashedPasswordRepository;
            this._userRepository = userRepository;
            this._forgotPasswordRepository = forgotPasswordRepository;
            this._exceptionRepository = exceptionRepository;
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
                    await this.InsertHashedPasswordMethodRecord(context, MethodBase.GetCurrentMethod().Name);
                    result = new OkObjectResult(new { HashedPassword = hashedPassword });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region BcryptVerify
        public async Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Password) && !string.IsNullOrEmpty(body.HashedPassword))
                {
                    BcryptWrapper wrapper = new BcryptWrapper();
                    bool valid = await wrapper.Verify(body.HashedPassword, body.Password);
                    await this.InsertHashedPasswordMethodRecord(context, MethodBase.GetCurrentMethod().Name);
                    result = new OkObjectResult(new { IsValid = valid });
                }
                else
                {
                    result = new BadRequestObjectResult(new { });
                }

            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region ForgotPassword
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
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
                    result = new OkObjectResult(new { message = "You should be expecting an email to reset your password soon." });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                User databaseUser = await this._userRepository.GetUserById(body.Id);
                if (databaseUser != null && body.Password.Equals(body.ConfirmPassword) && databaseUser.ForgotPassword.Token.Equals(body.Token))
                {

                    BcryptWrapper wrapper = new BcryptWrapper();
                    string hashedPassword = await wrapper.HashPasswordAsync(body.Password);
                    List<string> lastFivePasswords = await this._forgotPasswordRepository.GetLastFivePassword(body.Id);
                    foreach (string password in lastFivePasswords)
                    {
                        if (await wrapper.Verify(password, body.Password))
                        {
                            result = new BadRequestObjectResult(new { error = "You need to enter a password that hasn't been used the last 5 times" });
                            return result;
                        }
                    }
                    await this._userRepository.UpdatePassword(databaseUser.Id, hashedPassword);
                    await this._forgotPasswordRepository.InsertForgotPasswordAttempt(databaseUser.Id, hashedPassword);
                    result = new OkObjectResult(new { message = "You have successfully changed your password." });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region ScryptHash
        public async Task<IActionResult> ScryptHashPassword(ScryptHashRequest body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                SCryptWrapper scrypt = new SCryptWrapper();
                string hashedPassword = await scrypt.HashPasswordAsync(body.passwordToHash);
                await this.InsertHashedPasswordMethodRecord(context, MethodBase.GetCurrentMethod().Name);
                result = new OkObjectResult(new { hashedPassword = hashedPassword });
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

        #region ScryptVerify

        public async Task<IActionResult> ScryptVerifyPassword(SCryptVerifyRequest body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.hashedPassword) && !string.IsNullOrEmpty(body.password))
                {
                    SCryptWrapper scrypt = new SCryptWrapper();
                    bool isValid = await scrypt.VerifyPasswordAsync(body.password, body.hashedPassword);
                    await this.InsertHashedPasswordMethodRecord(context, MethodBase.GetCurrentMethod().Name);
                    result = new OkObjectResult(new { isValid = true });
                }
                else
                {
                    result = new BadRequestObjectResult(new { message = "You need to provide a hashed password and password to verify." });
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

        #region Helpers
        private async Task InsertHashedPasswordMethodRecord(HttpContext context, string methodName)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string userId = new JWT().GetUserIdFromToken(token);
            HashedPassword newPassword = new HashedPassword()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                UserId = userId,
                HashMethod = methodName,
                CreateDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            await this._hashedPasswordRepository.InsertOneHasedPassword(newPassword);
        }
        #endregion
    }
}
