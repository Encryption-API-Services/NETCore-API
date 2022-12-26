using API.ControllerLogic;
using Microsoft.AspNetCore.Mvc;

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
        

    }
}
