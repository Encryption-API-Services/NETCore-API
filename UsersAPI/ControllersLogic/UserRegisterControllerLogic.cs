using API.ControllersLogic;
using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Validation.UserRegistration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        private IUserRepository _userRespository { get; set; }
        private IMethodBenchmarkRepository _methodRespository { get; set; }
        public UserRegisterControllerLogic(IUserRepository userRepo, IMethodBenchmarkRepository methodRespository)
        {
            this._userRespository = userRepo;
            this._methodRespository = methodRespository;
        }

        #region RegisterUser
        public async Task<IActionResult> RegisterUser(RegisterUser body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            RegisterUserValidation validation = new RegisterUserValidation();
            if (validation.IsRegisterUserModelValid(body) && await this._userRespository.GetUserByEmail(body.email) == null)
            {
                await this._userRespository.AddUser(body);
                result = new OkObjectResult(new { message = "Successfully registered user" });
            }
            else
            {
                result = new BadRequestResult();
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
            logger.EndExecution();
            await this._methodRespository.InsertBenchmark(logger);
            return result;
        }

        #endregion
    }
}