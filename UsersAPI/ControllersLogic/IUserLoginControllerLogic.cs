using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IUserLoginControllerLogic
    {
        public Task<IActionResult> LoginUser(LoginUser body);
        public Task<IActionResult> GetRefreshToken(HttpContext context);
        public Task<IActionResult> UnlockUser(UnlockUser body, HttpContext context);
    }
}