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
                ECDSAWrapper ecdsa = new ECDSAWrapper("ES521");
                ecdsa.ImportFromPublicBase64String(publicKey);
                // validate signing key
                if (await new JWT().ValidateECCToken(token, ecdsa.ECDKey))
                {
                    // proceed to route logic that the JWT is actually protecting.
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
        private List<string> RoutesToValidate()
        {
            return new List<string>()
            {
                "/Encryption/EncryptAES",
                "/Encryption/DecryptAES",
                "/Encryption/EncryptSHA1",
                "/Encryption/EncryptSHA256",
                "/Encryption/EncryptSHA512",
                "/Credit/ValidateCard",
                "/Password/BCryptEncrypt",
                "/Password/BcryptVerify",
                "/Password/SCryptEncrypt",
                "/Password/SCryptVerify",
                "/Password/Argon2Hash",
                "/Password/Argon2Verify",
                "/TwoFA/Get2FAStatus",
                "/TwoFA/TurnOn2FA",
                "/TwoFA/TurnOff2FA",
                "/TwoFA/UpdatePhoneNumber",
                "/UserLogin/GetSuccessfulLogins",
                "/UserLogin/WasLoginMe",
                "/UserLogin/GetApiKey",
                "/Credit/AddCreditCard",
                "/Blog/CreatePost",
                "/Blog/UpdatePost",
                "/Blog/DeletePost",
                "/Rsa/GetKeyPair"
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
