using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFAController : ControllerBase
    {
        private readonly ITwoFAControllerLogic _twoFAControllerLogic;
        public TwoFAController(ITwoFAControllerLogic twoFAControllerLogic)
        {
            this._twoFAControllerLogic = twoFAControllerLogic;
        }

        [HttpGet]
        [Route("Get2FAStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> Get2FAStatus()
        {
            return await this._twoFAControllerLogic.Get2FAStatus(HttpContext);
        }

        [HttpPut]
        [Route("TurnOn2FA")]
        [AllowAnonymous]
        public async Task<IActionResult> TurnOn2FA()
        {
            return await this._twoFAControllerLogic.TurnOn2FA(HttpContext);
        }
    }
}
