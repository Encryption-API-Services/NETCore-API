using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace API.ControllerLogic
{
    public class TokenControllerLogic : ITokenControllerLogic
    {
        private readonly IUserRepository _userRepository;
        public TokenControllerLogic(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        #region GetToken
        public async Task<IActionResult> GetToken(HttpContext httpContext)
        {
            IActionResult result = null;
            string apiKey = httpContext.Request.Headers["ApiKey"].ToString();
            if (string.IsNullOrEmpty(apiKey))
            {
                result = new UnauthorizedObjectResult(new { error = "You did not set an ApiKey" });
            }
            User activeUser = await this._userRepository.GetUserByApiKey(apiKey);
            if (activeUser == null)
            {
                result = new UnauthorizedObjectResult(new { error = "You entered an invalid ApiKey"});
            }
            else if (activeUser.IsActive == false)
            {
                result = new UnauthorizedObjectResult(new { error = "This user account is no longer active" });
            }
            else
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                RSAParameters rsaParams = RSAalg.ExportParameters(true);
                string publicKey = RSAalg.ToXmlString(false);
                string token = new JWT().GenerateSecurityToken(activeUser.Id, rsaParams, publicKey, activeUser.IsAdmin);
                result = new OkObjectResult(new { token = token });
            }
            return result;
        }
        #endregion
    }
}