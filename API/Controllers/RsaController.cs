using API.ControllerLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RsaController : ControllerBase
    {
        private readonly IRsaControllerLogic _rsaControllerLogic; 
        public RsaController(IRsaControllerLogic rsaContollerLogic)
        {
            this._rsaControllerLogic = rsaContollerLogic;
        } 
        
        [HttpGet]
        [Route("GetKeyPair")]
        public async Task<IActionResult> GetKeyPair([FromQuery]int keySize)
        {
            return await this._rsaControllerLogic.GetKeyPair(HttpContext, keySize);
        }

        [HttpPost]
        [Route("EncryptWithoutPublic")]
        public async Task<IActionResult> EncryptWithoutPublic([FromBody]EncryptWithoutPublicRequest body)
        {
            return await this._rsaControllerLogic.EncryptWithoutPublic(HttpContext, body);
        }
    }
}