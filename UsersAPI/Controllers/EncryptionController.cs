using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UsersAPI.Controllers
{
    public class EncryptionController : Controller
    {

        public EncryptionController()
        {

        }

        [Route("EncryptAES")]
        // GET: EncryptionController/Details/5
        public async Task<IActionResult> EncryptAES()
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
