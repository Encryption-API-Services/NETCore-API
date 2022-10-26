using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Credit;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface ICreditControllerLogic
    {
        public Task<IActionResult> ValidateCreditCard([FromBody]CreditValidateRequest body, HttpContext httpContext);
    }
}