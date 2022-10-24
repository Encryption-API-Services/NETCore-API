using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IUserLoginControllerLogic
    {
        public Task<IActionResult> LoginUser(LoginUser body);
    }
}