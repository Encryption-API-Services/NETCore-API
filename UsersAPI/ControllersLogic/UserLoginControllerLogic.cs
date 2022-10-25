using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Mvc;
using Models.UserAuthentication;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public class UserLoginControllerLogic : IUserLoginControllerLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLoginControllerLogic(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public async Task<IActionResult> LoginUser(LoginUser body)
        {
            IActionResult result = null;
            try
            {
                User activeUser = await this._userRepository.GetUserByEmailAndPassword(body.Email, body.Password);
                if (activeUser != null)
                {
                    // TODO: abstract the RSAParameters to another class that contains the already exported public and private keys in XML to be save in database.
                    RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                    RSAParameters rsaParams = RSAalg.ExportParameters(true);
                    string privateKey = RSAalg.ToXmlString(true);
                    string token = new JWT().GenerateSecurityToken(activeUser.Id, rsaParams, privateKey);
                    JwtToken jwtToken = new JwtToken()
                    {
                        Token = token,
                        PrivateKey = RSAalg.ToXmlString(true),
                        PublicKey = RSAalg.ToXmlString(false)
                    };
                    await this._userRepository.UpdateUsersJwtToken(activeUser, jwtToken);
                    result = new OkObjectResult(new { token = token });
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
