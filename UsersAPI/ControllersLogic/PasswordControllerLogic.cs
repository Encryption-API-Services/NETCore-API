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

namespace UsersAPI.ControllersLogic
{
    public class PasswordControllerLogic : IPasswordControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        public PasswordControllerLogic(IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }

        public async Task<IActionResult> BcryptEncryptPassword([FromBody]BCryptEncryptModel body, HttpContext context)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(context);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.Password))
                {
                    BcryptWrapper bcrypt = new BcryptWrapper();
                    string hashedPassword = await bcrypt.HashPasswordAsync(body.Password);

                    result = new OkObjectResult(new { hashedPassword = hashedPassword });
                }
            }
            catch(Exception ex)
            {
                result = new BadRequestObjectResult(new {});
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
    }
}
