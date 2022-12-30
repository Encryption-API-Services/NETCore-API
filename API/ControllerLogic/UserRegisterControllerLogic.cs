using API.ControllersLogic;
using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption.PasswordHash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.UserAuthentication;
using Payments;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Validation.UserRegistration;
using User = DataLayer.Mongo.Entities.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Config
{
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        private IUserRepository _userRespository;
        private IMethodBenchmarkRepository _methodRespository;
        private ILogRequestRepository _logRequestRespository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public UserRegisterControllerLogic(
            IUserRepository userRepo, 
            IMethodBenchmarkRepository methodRespository, 
            ILogRequestRepository logRequestRespository,
            IEASExceptionRepository exceptionRespitory
            )
        {
            this._userRespository = userRepo;
            this._methodRespository = methodRespository;
            this._logRequestRespository = logRequestRespository;
            this._exceptionRepository = exceptionRespitory;
        }

        #region RegisterUser
        public async Task<IActionResult> RegisterUser(RegisterUser body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                // Limit of 10 requests per hour for registering user by IP.
                List<LogRequest> requests = await this._logRequestRespository.GetTop10RequestsByIP((string)context.Items["IP"]);
                if (requests.Count <= 10)
                {
                    RegisterUserValidation validation = new RegisterUserValidation();
                    Task<User> emailUser = this._userRespository.GetUserByEmail(body.email);
                    Task<User> usernameUser = this._userRespository.GetUserByUsername(body.username);
                    await Task.WhenAll(emailUser, usernameUser);
                    if (validation.IsRegisterUserModelValid(body) && emailUser.Result == null && usernameUser.Result == null)
                    {
                        Argon2Wrappper argon2 = new Argon2Wrappper();
                        string hashedPassword = await argon2.HashPasswordAsync(body.password);
                        await this._userRespository.AddUser(body, hashedPassword);
                        Argon2Wrappper.free_cstring_memory(hashedPassword);
                        result = new OkObjectResult(new { message = "Successfully registered user" });
                    }
                    else
                    {
                        result = new BadRequestObjectResult(new {error = "This email and or username already exists"});
                    }
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "You have made to many requests in the last hour." });
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our side" });
            }
            logger.EndExecution();
            await this._methodRespository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region ActivateUser

        public async Task<IActionResult> ActivateUser(ActivateUser body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                User userToActivate = await this._userRespository.GetUserById(body.Id);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
                rsa.FromXmlString(userToActivate.EmailActivationToken.PrivateKey);
                byte[] decodedUrl = Base64UrlEncoder.DecodeBytes(body.Token);
                if (rsa.VerifyData(Encoding.UTF8.GetBytes(userToActivate.EmailActivationToken.Token), SHA512.Create(), decodedUrl))
                {
                    StripCustomer stripCustomer = new StripCustomer();
                    string stripCustomerId = await stripCustomer.CreateStripCustomer(userToActivate);
                    await this._userRespository.ChangeUserActiveById(userToActivate, true, stripCustomerId);
                    result = new OkObjectResult(new { message = "User account was successfully activated." });
                }
                else
                {
                    result = new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our side." });
            }
            logger.EndExecution();
            await this._methodRespository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}