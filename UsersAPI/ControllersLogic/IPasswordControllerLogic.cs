using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IPasswordControllerLogic
    {
        Task<IActionResult> BcryptEncryptPassword([FromBody] BCryptEncryptModel body, HttpContext context);
        Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body, HttpContext context);
    }
}
