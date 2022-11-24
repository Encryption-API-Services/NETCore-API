using API.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private IUserRegisterControllerLogic _userRegisterLogic { get; set; }

        public UserRegisterController(IUserRegisterControllerLogic userRegisterLogic)
        {
            this._userRegisterLogic = userRegisterLogic;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterUser body)
        {
            return await this._userRegisterLogic.RegisterUser(body, HttpContext);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("Activate")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ActivateUser body)
        {
            return await this._userRegisterLogic.ActivateUser(body, HttpContext);
        }
    }
}
