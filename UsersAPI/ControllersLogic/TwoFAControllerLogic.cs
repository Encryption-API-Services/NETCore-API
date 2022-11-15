using Common;
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
        public TwoFAControllerLogic(IMethodBenchmarkRepository methodBenchmarkRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepository;
        }
        public async Task<IActionResult> TurnOn2FA(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                 
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
