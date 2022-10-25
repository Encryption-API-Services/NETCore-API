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
                RSACryptoServiceProvider rsaProvdier = new RSACryptoServiceProvider(4096);
                rsaProvdier.FromXmlString(signingKey);
                RSAParameters parameters = rsaProvdier.ExportParameters(true);
                bool isValid = await new JWT().ValidateSecurityToken(userId, token, parameters);
            }

            await _next(context);
        }
        private List<string> RoutesToValidate()
        {
            return new List<string>()
            {
                "/api/UserLogin/"
            };
        }
    }
}
