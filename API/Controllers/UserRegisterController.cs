using API.ControllersLogic;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private UserRegisterControllerLogic _userRegisterLogic { get; set; }

        public UserRegisterController(UserRegisterControllerLogic userRegisterLogic)
        {
            this._userRegisterLogic = userRegisterLogic;
        }

        // GET: api/<UserRegisterController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserRegisterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserRegisterController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserRegisterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRegisterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
