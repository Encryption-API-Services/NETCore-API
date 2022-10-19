using API.ControllersLogic;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;
using Validation.UserRegistration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        private IUserRepository _userRespository { get; set; }
        public UserRegisterControllerLogic(IUserRepository userRepo)
        {
            this._userRespository = userRepo;
        }
        public async Task<IActionResult> RegisterUser(RegisterUser body)
        {
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
            return result;
        }
    }
}