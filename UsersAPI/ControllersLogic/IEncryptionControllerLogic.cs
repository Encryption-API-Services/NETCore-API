using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IEncryptionControllerLogic
    {
        public Task<IActionResult> EncryptAES(EncryptAESRequest body);
    }
}
