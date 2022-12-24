using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using Models.UserAuthentication;

namespace API.ControllersLogic
{
    public interface IPasswordControllerLogic
    {
        Task<IActionResult> BcryptEncryptPassword([FromBody] BCryptEncryptModel body, HttpContext context);
        Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body, HttpContext context);
        Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest email, HttpContext context);
        Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest body, HttpContext context);
        Task<IActionResult> ScryptHashPassword(ScryptHashRequest body, HttpContext context);
        Task<IActionResult> ScryptVerifyPassword(SCryptVerifyRequest body, HttpContext context);
    }
}
