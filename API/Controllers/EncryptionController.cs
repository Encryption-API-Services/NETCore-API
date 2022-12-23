using API.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncryptionControllerLogic _encryptionControllerLogic;
        public EncryptionController(IEncryptionControllerLogic controllerLogic)
        {
            this._encryptionControllerLogic = controllerLogic;
        }

        [HttpPost]
        [Route("EncryptAES")]
        public async Task<IActionResult> EncryptAES([FromBody] EncryptAESRequest body)
        {
            return await this._encryptionControllerLogic.EncryptAES(body, HttpContext);
        }

        [HttpPost]
        [Route("DecryptAES")]
        public async Task<IActionResult> DecryptAES([FromBody] DecryptAESRequest body)
        {
            return await this._encryptionControllerLogic.DecryptAES(body, HttpContext);
        }

        [HttpPost]
        [Route("EncryptSHA1")]
        public async Task<IActionResult> EncryptSHA1([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA(body, HttpContext, Encryption.SHATypes.SHA1);
        }

        [HttpPost]
        [Route("EncryptSHA256")]
        public async Task<IActionResult> EncryptSHA256([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA(body, HttpContext, Encryption.SHATypes.SHA256);
        }

        [HttpPost]
        [Route("EncryptSHA512")]
        public async Task<IActionResult> EncryptSHA512([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA(body, HttpContext, Encryption.SHATypes.SHA512);
        }

        [HttpPost]
        [Route("HashMD5")]
        public async Task<IActionResult> HashMD5([FromBody] MD5Request body)
        {
            return await this._encryptionControllerLogic.HashMD5(body, HttpContext);
        }
    }
}