using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;

namespace API.ControllersLogic
{
    public interface IUserRegisterControllerLogic
    {
        public Task<IActionResult> RegisterUser(RegisterUser body, HttpContext context);
        public Task<IActionResult> ActivateUser(ActivateUser body, HttpContext context);
    }
}
