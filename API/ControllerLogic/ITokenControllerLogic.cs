using Microsoft.AspNetCore.Mvc;

namespace API.ControllerLogic
{
    public interface ITokenControllerLogic
    {
        public Task<IActionResult> GetToken(string apiKey);
    }
}
