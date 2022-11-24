using DataLayer.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserLoginControllerLogic _loginControllerLogic;
        private readonly LogRequestService _logRequestService;

        public UserLoginController(IUserLoginControllerLogic loginControllerLogic, LogRequestService logRequestService)
        {
            this._loginControllerLogic = loginControllerLogic;
            this._logRequestService = logRequestService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        // POST: UserLoginController
        public async Task<IActionResult> LoginUser([FromBody]LoginUser body)
        {
            return await this._loginControllerLogic.LoginUser(body, HttpContext);
        }

        [HttpGet]
        [Route("RefreshToken")]
        public async Task<IActionResult> GetRefreshToken()
        {
            return await this._loginControllerLogic.GetRefreshToken(HttpContext);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut]
        [Route("UnlockUser")]
        public async Task<IActionResult> UnlockUser([FromBody] UnlockUser body)
        {
            return await this._loginControllerLogic.UnlockUser(body, HttpContext);
        }

        [HttpPut]
        [Route("ValidateHotpCode")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateHotpCode([FromBody] ValidateHotpCode body)
        {
            return await this._loginControllerLogic.ValidateHotpCode(body, HttpContext);
        }

        [HttpGet]
        [Route("GetSuccessfulLogins")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSuccessfulLogins()
        {
            return await this._loginControllerLogic.GetSuccessfulLogins(HttpContext);
        }

        [HttpPost]
        [Route("WasLoginMe")]
        [AllowAnonymous]
        public async Task<IActionResult> WasLoginMe([FromBody]WasLoginMe body)
        {
            return await this._loginControllerLogic.WasLoginMe(body, HttpContext);
        }
    }
}