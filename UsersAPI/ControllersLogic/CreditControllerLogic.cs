using Common;
using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Credit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Validation.CreditCard;

namespace UsersAPI.ControllersLogic
{
    public class CreditControllerLogic : ICreditControllerLogic
    {
        private readonly ICreditRepository _creditRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        public CreditControllerLogic(ICreditRepository creditRepository, IHttpContextAccessor contextAccessor, IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._creditRepository = creditRepository;
            this._contextAccessor = contextAccessor;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }
        public async Task<IActionResult> ValidateCreditCard([FromBody] CreditValidateRequest body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                if (!string.IsNullOrEmpty(body.CCNumber))
                {
                    LuhnWrapper wrapper = new LuhnWrapper();
                    bool isValidCC = await wrapper.IsCCValidAsync(body.CCNumber);
                    if (isValidCC)
                    {
                        // get user id from token
                        string userId = this._contextAccessor.HttpContext.Items["UserID"].ToString();
                        await this._creditRepository.AddValidatedCreditInformation(new ValidatedCreditCard()
                        {
                            UserID = userId,
                            CreationTime = DateTime.UtcNow,
                            LastModifiedTime = DateTime.UtcNow
                        });
                        result = new OkObjectResult(new { IsValid = true });
                    }
                    else
                    {
                        result = new OkObjectResult(new { IsValid = false });
                    }
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestResult();
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
    }
}