using Common;
using DataLayer.Mongo.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Validation.CreditCard;
using Encryption;
using DataLayer.Mongo.Repositories;
using MongoDB.Bson;

namespace UsersAPI.ControllersLogic
{
    public class PasswordControllerLogic : IPasswordControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IHashedPasswordRepository _hashedPasswordRepository;
        public PasswordControllerLogic(IMethodBenchmarkRepository methodBenchmarkRepository, IHashedPasswordRepository hashedPasswordRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            _hashedPasswordRepository = hashedPasswordRepository;
        }

        public async Task<IActionResult> BcryptEncryptPassword([FromBody] BCryptEncryptModel body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Password))
                {
                    BcryptWrapper bcrypt = new BcryptWrapper();
                    string hashedPassword = await bcrypt.HashPasswordAsync(body.Password);
                    string id = ObjectId.GenerateNewId().ToString();
                    HashedPassword newPassword = new HashedPassword()
                    {
                        Id = id,
                        Password = hashedPassword,
                        CreateDate = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow
                    };
                    await this._hashedPasswordRepository.InsertOneHasedPassword(newPassword);
                    result = new OkObjectResult(new { hashedPassword = hashedPassword, ID = id });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        public async Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body, HttpContext context)
        {
            IActionResult result = null;
            HashedPassword newPassword = await this._hashedPasswordRepository.GetOneHashedPassword(body.ID);
            if (newPassword != null)
            {
                BcryptWrapper wrapper = new BcryptWrapper();
                bool valid = await wrapper.Verify(newPassword.Password, body.Password);
                result = new OkObjectResult(new { IsValid = valid });
            }
            else
            {
                result = new BadRequestObjectResult(new { });
            }
            return result;
        }
    }
}
