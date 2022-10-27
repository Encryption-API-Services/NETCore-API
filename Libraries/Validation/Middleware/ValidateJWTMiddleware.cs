using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Middleware
{
    public class ValidateJWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDatabaseSettings _settings;
        private readonly IHttpContextAccessor _contextAccessor;

        public ValidateJWTMiddleware(RequestDelegate next, IDatabaseSettings databaseSettings)
        {
            _next = next;
            this._settings = databaseSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string routePath = context.Request.Path;
            if (token != null && RoutesToValidate().Contains(routePath))
            {
                var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);
                string signingKey = handler.Claims.First(x => x.Type == "private-key").Value;
                string userId = handler.Claims.First(x => x.Type == "id").Value;
                context.Items["UserID"] = userId;
                RSACryptoServiceProvider rsaProvdier = new RSACryptoServiceProvider(4096);
                rsaProvdier.FromXmlString(signingKey);
                RSAParameters parameters = rsaProvdier.ExportParameters(true);
                UserRepository userRepository = new UserRepository(this._settings);
                User user = await userRepository.GetUserById(userId);

                //compare database key to key in token
                if (user.JwtToken.PrivateKey.Equals(signingKey))
                {
                    // validate signing key
                    if (await new JWT().ValidateSecurityToken(token, parameters))
                    {
                        // proceed to route logic that the JWT is actually protecting.
                        await _next(context);
                    }
                }
            }
            await _next(context);
        }
        private List<string> RoutesToValidate()
        {
            return new List<string>()
            {
                "/api/Encryption/EncryptAES/",
                "/api/Encryption/DecryptAES/",
                "/api/Encryption/EncryptSHA1/",
                "/api/Encryption/EncryptSHA256/",
                "/api/Encryption/EncryptSHA512/",
                "/api/Credit/ValidateCard",
                "/api/Password/BCryptEncrypt",
                "/api/Password/BcryptVerify"
            };
        }
    }
}
