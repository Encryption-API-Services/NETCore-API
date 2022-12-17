using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        // GET: api/<TokenController>
        [HttpGet]
        public IEnumerable<string> GetToken([FromQuery]string apiToken)
        {
            return new string[] { "value1", "value2" };
        }       
    }
}