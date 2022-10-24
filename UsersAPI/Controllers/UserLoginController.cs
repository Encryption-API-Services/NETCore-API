using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {
        private readonly IUserLoginControllerLogic _loginControllerLogic;

        public UserLoginController(IUserLoginControllerLogic loginControllerLogic)
        {

        }

        [HttpPost]
        // GET: UserLoginController
        public async Task<IActionResult> LoginUser()
        {
            return View();
        }
    }
}