using Microsoft.AspNetCore.Mvc;

namespace API.ControllerLogic
{
    public interface IRsaControllerLogic
    {
        Task<IActionResult> GetKeyPair(HttpContext context, int keySize);
    }
}
