using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Encryption;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string routePath = context.Request.Path;
            if (token != null && this.RoutesToValidate().Contains(routePath))
            {
                var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);
                string publicKey = handler.Claims.First(x => x.Type == "public-key").Value;
                string userId = handler.Claims.First(x => x.Type == "id").Value;
                context.Items["UserID"] = userId;
                UserRepository userRepository = new UserRepository(this._settings);
                User user = await userRepository.GetUserByIdAndPublicKey(userId, publicKey);
                RSACryptoServiceProvider rsaProvdier = new RSACryptoServiceProvider(4096);
                rsaProvdier.FromXmlString(user.JwtToken.PrivateKey);
                RSAParameters parameters = rsaProvdier.ExportParameters(true);

                //compare database public key to public key in token
                if (user.JwtToken.PublicKey.Equals(publicKey))
                {
                    // validate signing key
                    if (await new JWT().ValidateSecurityToken(token, parameters))
                    {
                        // proceed to route logic that the JWT is actually protecting.
                        await _next(context);
                    }
                }
            }
        }
        private List<string> RoutesToValidate()
        {
            return new List<string>()
            {
                "/api/Encryption/EncryptAES",
                "/api/Encryption/DecryptAES",
                "/api/Encryption/EncryptSHA1",
                "/api/Encryption/EncryptSHA256",
                "/api/Encryption/EncryptSHA512",
                "/api/Credit/ValidateCard",
                "/api/Password/BCryptEncrypt",
                "/api/Password/BcryptVerify",
                "/api/TwoFA/Get2FAStatus",
                "/api/TwoFA/TurnOn2FA",
                "/api/TwoFA/TurnOff2FA",
                "/api/TwoFA/UpdatePhoneNumber",
                "/api/UserLogin/GetSuccessfulLogins",
                "/api/UserLogin/WasLoginMe"
            };
        }
    }

    public static class ValidateJWTMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ValidateJWTMiddleware>();
        }
    }
}
