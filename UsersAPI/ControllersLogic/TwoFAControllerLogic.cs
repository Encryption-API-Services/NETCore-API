using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public class TwoFAControllerLogic : ITwoFAControllerLogic
    {
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IUserRepository _userRepository;
        public TwoFAControllerLogic(IMethodBenchmarkRepository methodBenchmarkRepository, IUserRepository userRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._userRepository = userRepository;
        }

        public async Task<IActionResult> Get2FAStatus(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string userId = httpContext.Items["UserID"].ToString();
                Phone2FA status = await this._userRepository.GetPhone2FAStats(userId);
                result = new OkObjectResult(new { result = status.IsEnabled });
            }
            catch (Exception ex)
            {

            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        public async Task<IActionResult> TurnOn2FA(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string userId = httpContext.Items["UserID"].ToString(); 
            }
            catch (Exception ex)
            {

            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
    }
}
