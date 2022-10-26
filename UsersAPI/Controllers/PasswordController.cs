using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : Controller
    {
        private readonly IPasswordControllerLogic _passwordControllerLogic;
        public PasswordController(IPasswordControllerLogic passwordControllerLogic)
        {
            this._passwordControllerLogic = passwordControllerLogic;
        }

        [HttpPost]
        [Route("BCryptEncrypt")]
        public async Task<IActionResult> BcryptPassword([FromBody]BCryptEncryptModel body)
        {
            return await this._passwordControllerLogic.BcryptEncryptPassword(body, HttpContext);
        }
    }
}