using Microsoft.AspNetCore.Mvc;
using Models.Encryption;

namespace API.ControllersLogic
{
    public interface IEncryptionControllerLogic
    {
        public Task<IActionResult> EncryptAES(EncryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> DecryptAES(DecryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> EncryptSHA512(EncryptSHARequest body, HttpContext httpContext);
        public Task<IActionResult> HashMD5(MD5Request body, HttpContext httpContext);
    }
}
