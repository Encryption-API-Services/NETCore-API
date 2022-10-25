using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : Controller
    {

        public EncryptionController()
        {

        }

        [Route("EncryptAES")]
        // GET: EncryptionController/Details/5
        public async Task<IActionResult> EncryptAES([FromBody]EncryptAESRequest body)
        {
            return Ok();
        }

        [Route("DecryptAES")]
        public async Task<IActionResult> DecryptAES()
        {
            return Ok();
        }
    }
}
