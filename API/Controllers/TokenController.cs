using API.ControllerLogic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenControllerLogic _tokenControllerLogic;
        public TokenController(ITokenControllerLogic tokenControllerLogic)
        {
            this._tokenControllerLogic = tokenControllerLogic;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            return await this._tokenControllerLogic.GetToken(HttpContext);
        }       
    }
}