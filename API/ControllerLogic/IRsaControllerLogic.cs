using Microsoft.AspNetCore.Mvc;
using Models.Encryption;

namespace API.ControllerLogic
{
    public interface IRsaControllerLogic
    {
        Task<IActionResult> GetKeyPair(HttpContext context, int keySize);
        Task<IActionResult> EncryptWithoutPublic(HttpContext context, EncryptWithoutPublicRequest body);
    }
}