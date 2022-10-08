using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Config
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterControllerLogic : IUserRegisterControllerLogic
    {
        // GET: api/<UserRegisterControllerLogic>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/<UserRegisterControllerLogic>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserRegisterControllerLogic>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<UserRegisterControllerLogic>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
