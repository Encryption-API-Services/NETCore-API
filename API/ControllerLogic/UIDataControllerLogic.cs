﻿using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace API.ControllerLogic
{
    public class UIDataControllerLogic : IUIDataControllerLogic
    {
        private IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public UIDataControllerLogic(
            IMethodBenchmarkRepository methodBenchmarkRepo, 
            IEASExceptionRepository exceptionRepository)
        {
            this._methodBenchmarkRepository = methodBenchmarkRepo;
            this._exceptionRepository = exceptionRepository;
        }

        public async Task<IActionResult> GetHomePageBenchMarkData(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                List<BenchmarkMethod> methodsToDisplay = await this._methodBenchmarkRepository.GetAmountByEndTimeDescending(25);
                result = new OkObjectResult(new { data = methodsToDisplay });
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end" });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
    }
}