using Common;
using Common.ThirdPartyAPIs;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Encryption.PasswordHash;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using OtpNet;
using System.Reflection;

namespace API.ControllersLogic
{
    public class UserLoginControllerLogic : IUserLoginControllerLogic
    {
        private readonly IUserRepository _userRepository;
        private readonly IFailedLoginAttemptRepository _failedLoginAttemptRepository;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IHotpCodesRepository _hotpCodesRepository;
        private readonly ISuccessfulLoginRepository _successfulLoginRepository;
        private readonly IEASExceptionRepository _exceptionRepository;

        public UserLoginControllerLogic(
            IUserRepository userRepository,
            IFailedLoginAttemptRepository failedLoginAttemptRepository,
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IHotpCodesRepository hotpCodesRepository,
            ISuccessfulLoginRepository successfulLoginRepository,
            IEASExceptionRepository exceptionRepository)
        {
            this._userRepository = userRepository;
            this._failedLoginAttemptRepository = failedLoginAttemptRepository;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._hotpCodesRepository = hotpCodesRepository;
            this._successfulLoginRepository = successfulLoginRepository;
            this._exceptionRepository = exceptionRepository;
        }
        
        #region GetApiKey
        public async Task<IActionResult> GetApiKey(HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                // get current token
                if (!string.IsNullOrEmpty(token))
                {
                    JWT jwtWrapper = new JWT();
                    string userId = jwtWrapper.GetUserIdFromToken(token);
                    string apiKey = await this._userRepository.GetApiKeyById(userId);
                    result = new OkObjectResult(new { apiKey = apiKey });
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

        #region GetSuccessfulLogins
        public async Task<IActionResult> GetSuccessfulLogins(HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                string userId = context.Items["UserID"].ToString();
                List<SuccessfulLogin> successfulLogins = await this._successfulLoginRepository.GetAllSuccessfulLoginWithinTimeFrame(userId, DateTime.UtcNow.AddMonths(-1));
                result = new OkObjectResult(new { logins = successfulLogins });
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end getting the recent login activity." });
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
                    Argon2Wrappper argon2 = new Argon2Wrappper();
                    if (await argon2.VerifyPasswordAsync(activeUser.Password, body.Password))
                    {
                        ECDSAWrapper ecdsa = new ECDSAWrapper("ES521");
                        string token = new JWT().GenerateECCToken(activeUser.Id, activeUser.IsAdmin, ecdsa);

                        if (activeUser.Phone2FA != null && activeUser.Phone2FA.IsEnabled)
                        {
                            byte[] secretKey = KeyGeneration.GenerateRandomKey(OtpHashMode.Sha512);
                            long counter = await this._hotpCodesRepository.GetHighestCounter() + 1;
                            Hotp hotpGenerator = new Hotp(secretKey, OtpHashMode.Sha512, 8);
                            HotpCode code = new HotpCode()
                            {
                                UserId = activeUser.Id,
                                Counter = counter,
                                Hotp = hotpGenerator.ComputeHOTP(counter),
                                HasBeenSent = false,
                                HasBeenVerified = false
                            };
                            await this._hotpCodesRepository.InsertHotpCode(code);
                            result = new OkObjectResult(new { message = "You need to verify the code sent to your phone.", TwoFactorAuth = true });
                        }
                        else
                        {
                            IpInfoHelper ipInfoHelper = new IpInfoHelper();
                            IpInfoResponse ipInfo = await ipInfoHelper.GetIpInfo(httpContext.Items["IP"].ToString());
                            SuccessfulLogin login = new SuccessfulLogin()
                            {
                                UserId = activeUser.Id,
                                Ip = httpContext.Items["IP"].ToString(),
                                UserAgent = body.UserAgent,
                                City = ipInfo.City,
                                Country = ipInfo.Country,
                                TimeZone = ipInfo.TimeZone,
                                CreateTime = DateTime.UtcNow
                            };
                            await this._successfulLoginRepository.InsertSuccessfulLogin(login);
                            result = new OkObjectResult(new { message = "You have successfully signed in.", token = token, TwoFactorAuth = false });
                        }
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
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
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
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Id))
                {
                    await this._userRepository.UnlockUser(body.Id);
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our side" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        public async Task<IActionResult> ValidateHotpCode([FromBody] ValidateHotpCode body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                // get hotp code by userId and HotpCode
                HotpCode databaseCode = await this._hotpCodesRepository.GetHotpCodeByIdAndCode(body.UserId, body.HotpCode);
                if (databaseCode != null && databaseCode.Hotp.Equals(body.HotpCode) && databaseCode.UserId.Equals(body.UserId))
                {
                    await this._hotpCodesRepository.UpdateHotpToVerified(databaseCode.Id);
                    result = new OkObjectResult(new { message = "You have successfully verified your authentication code." });
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "The authentication code that you entered was invalid" });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our side" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        #endregion

        #region WasLoginMe
        public async Task<IActionResult> WasLoginMe(WasLoginMe body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                await this._successfulLoginRepository.UpdateSuccessfulLoginWasMe(body.LoginId, body.WasMe);
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
