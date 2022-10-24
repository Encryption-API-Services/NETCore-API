using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IUserLoginControllerLogic
    {
        public Task<IActionResult> LoginUser();
    }
}