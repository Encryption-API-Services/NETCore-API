using API.ControllersLogic;
using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Models.UserAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Validation.UserRegistration;
using User = DataLayer.Mongo.Entities.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        private IUserRepository _userRespository { get; set; }
        private IMethodBenchmarkRepository _methodRespository { get; set; }
        private ILogRequestRepository _logRequestRespository { get; set; }
        public UserRegisterControllerLogic(IUserRepository userRepo, IMethodBenchmarkRepository methodRespository, ILogRequestRepository logRequestRespository)
        {
            this._userRespository = userRepo;
            this._methodRespository = methodRespository;
            this._logRequestRespository = logRequestRespository;
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
                        await this._userRespository.AddUser(body);
                        result = new OkObjectResult(new { message = "Successfully registered user" });
                    }
                    else
                    {
                        result = new BadRequestResult();
                    }
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "You have made to many requests in the last hour." });
                }
            }
            catch (Exception ex)
            {
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
                if (body.Token.Equals(userToActivate.EmailActivationToken.Token) && rsa.VerifyData(Encoding.UTF8.GetBytes(userToActivate.EmailActivationToken.Token), SHA512.Create(), userToActivate.EmailActivationToken.SignedToken))
                {
                    await this._userRespository.ChangeUserActiveById(userToActivate, true);
                    result = new OkObjectResult(new { message = "User account was successfully activated." });
                }
                else
                {
                    result = new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "There was an error on our side." });
            }
            logger.EndExecution();
            await this._methodRespository.InsertBenchmark(logger);
            return result;
        }

        #endregion
    }
}