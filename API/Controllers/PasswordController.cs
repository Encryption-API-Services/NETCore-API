using API.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using Models.UserAuthentication;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordControllerLogic _passwordControllerLogic;
        public PasswordController(IPasswordControllerLogic passwordControllerLogic)
        {
            this._passwordControllerLogic = passwordControllerLogic;
        }

        [HttpPost]
        [Route("BCryptEncrypt")]
        public async Task<IActionResult> BcryptPassword([FromBody] BCryptEncryptModel body)
        {
            return await this._passwordControllerLogic.BcryptEncryptPassword(body, HttpContext);
        }

        [HttpPost]
        [Route("BcryptVerify")]
        public async Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body)
        {
            return await this._passwordControllerLogic.BcryptVerifyPassword(body, HttpContext);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest body)
        {
            return await this._passwordControllerLogic.ForgotPassword(body, HttpContext);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body)
        {
            return await this._passwordControllerLogic.ResetPassword(body, HttpContext);
        }

        [HttpPost]
        [Route("SCryptEncrypt")]
        public async Task<IActionResult> SCryptEncrypt([FromBody]ScryptHashRequest body)
        {
            return await this._passwordControllerLogic.ScryptHashPassword(body, HttpContext);
        }
    }
}