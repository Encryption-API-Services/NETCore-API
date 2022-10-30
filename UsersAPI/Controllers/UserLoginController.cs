using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {
        private readonly IUserLoginControllerLogic _loginControllerLogic;

        public UserLoginController(IUserLoginControllerLogic loginControllerLogic)
        {
            this._loginControllerLogic = loginControllerLogic;
        }

        [HttpPost]
        // POST: UserLoginController
        public async Task<IActionResult> LoginUser([FromBody]LoginUser body)
        {
            return await this._loginControllerLogic.LoginUser(body);
        }

        [HttpGet]
        [Route("RefreshToken")]
        public async Task<IActionResult> GetRefreshToken()
        {
            return await this._loginControllerLogic.GetRefreshToken(HttpContext);
        }
    }
}