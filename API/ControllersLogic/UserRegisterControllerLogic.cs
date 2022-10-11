using API.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;
using Validation.UserRegistration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        public async Task<IActionResult> RegisterUser(RegisterUser body)
        {
            IActionResult result = null;
            RegisterUserValidation validation = new RegisterUserValidation();
            if (validation.IsRegisterUserModelValid(body))
            {
                
            }
            else
            {
                result = new BadRequestResult();
            }
            return result;
        }
    }
}