using Microsoft.AspNetCore.Mvc;
using Models.Credit;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : Controller
    {
        private readonly ICreditControllerLogic _creditControllerLogic;

        public CreditController(ICreditControllerLogic creditControllerLogic)
        {
            this._creditControllerLogic = creditControllerLogic;
        }

        [HttpPost]
        [Route("ValidateCard")]
        // GET: CreditController
        public async Task<IActionResult> ValidateCreditCard([FromBody] CreditValidateRequest body)
        {
            return await this._creditControllerLogic.ValidateCreditCard(body, HttpContext);
        }
    }
}