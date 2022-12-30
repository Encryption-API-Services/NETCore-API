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
        public async Task<IActionResult> GetKeyPair([FromQuery] int keySize)
        {
            return await this._rsaControllerLogic.GetKeyPair(HttpContext, keySize);
        }

        [HttpPost]
        [Route("EncryptWithoutPublic")]
        public async Task<IActionResult> EncryptWithoutPublic([FromBody] EncryptWithoutPublicRequest body)
        {
            return await this._rsaControllerLogic.EncryptWithoutPublic(HttpContext, body);
        }

        [HttpPost]
        [Route("EncryptWithPublic")]
        public async Task<IActionResult> EncryptWithPublic([FromBody] EncryptWithPublicRequest body)
        {
            return await this._rsaControllerLogic.EncryptWithPublic(HttpContext, body);
        }

        [HttpPost]
        [Route("Decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] RsaDecryptRequest body)
        {
            return await this._rsaControllerLogic.Decrypt(HttpContext, body);
        }

        [HttpPost]
        [Route("SignWithoutKey")]
        public async Task<IActionResult> SignWithoutKey([FromBody]RsaSignWithoutKeyRequest body)
        {
            return await this._rsaControllerLogic.SignWithoutKey(HttpContext, body);
        }

        [HttpPost]
        [Route("SignWithKey")]
        public async Task<IActionResult> SignWithKey([FromBody]RsaSignWithKeyRequest body)
        {
            return await this._rsaControllerLogic.VerifyWithKey(HttpContext, body);
        }

        [HttpPost]
        [Route("Verify")]
        public async Task<IActionResult> Verify([FromBody] RsaVerifyRequest body)
        {
            return await this._rsaControllerLogic.Verify(HttpContext, body);
        }
    }
}