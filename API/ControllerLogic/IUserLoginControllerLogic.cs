using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;

namespace API.ControllersLogic
{
    public interface IUserLoginControllerLogic
    {
        public Task<IActionResult> LoginUser(LoginUser body, HttpContext httpContext);
        public Task<IActionResult> GetRefreshToken(HttpContext context);
        public Task<IActionResult> UnlockUser(UnlockUser body, HttpContext context);
        public Task<IActionResult> ValidateHotpCode([FromBody] ValidateHotpCode body, HttpContext context);
        public Task<IActionResult> GetSuccessfulLogins(HttpContext context);
        public Task<IActionResult> WasLoginMe(WasLoginMe body, HttpContext context);
    }
}