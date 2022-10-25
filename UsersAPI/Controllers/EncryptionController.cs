using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : Controller
    {
        private readonly IEncryptionControllerLogic _encryptionControllerLogic;
        public EncryptionController(IEncryptionControllerLogic controllerLogic)
        {
            this._encryptionControllerLogic = controllerLogic;
        }

        [HttpPost]
        [Route("EncryptAES")]
        // GET: EncryptionController/Details/5
        public async Task<IActionResult> EncryptAES([FromBody] EncryptAESRequest body)
        {
            return await this._encryptionControllerLogic.EncryptAES(body);
        }

        [HttpPost]
        [Route("DecryptAES")]
        public async Task<IActionResult> DecryptAES([FromBody] DecryptAESRequest body)
        {
            return await this._encryptionControllerLogic.DecryptAES(body);
        }
    }
}