using Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IEncryptionControllerLogic
    {
        public Task<IActionResult> EncryptAES(EncryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> DecryptAES(DecryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> EncryptSHA(EncryptSHARequest body, HttpContext httpContext, SHATypes type);
        public Task<IActionResult> HashMD5(MD5Request body, HttpContext httpContext);
    }
}
