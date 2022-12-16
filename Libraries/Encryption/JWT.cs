using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    public class JWT
    {


        public JWT()
        {

        }
        public string GenerateSecurityToken(string userId, RSAParameters rsaParameters, string publicKey, bool isAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", userId), 
                    new Claim("public-key", publicKey),
                    new Claim("IsAdmin", isAdmin.ToString())
                }),
                Issuer = "https://www.encryptionapiservices.com",
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsaParameters), SecurityAlgorithms.RsaSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public async Task<bool> ValidateSecurityToken(string token, RSAParameters rsaParams)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationResult tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new RsaSecurityKey(rsaParams),
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = "https://www.encryptionapiservices.com",
                ClockSkew = TimeSpan.Zero
            });
            return tokenValidationResult.IsValid;
        }

        public string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return handler.Claims.First(x => x.Type == "id").Value;
        }
    }
}

