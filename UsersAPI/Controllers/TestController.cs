using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "That test worked";
        }
    }
}
