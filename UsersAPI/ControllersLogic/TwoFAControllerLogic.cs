using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.TwoFactorAuthentication;
using System;
using System.Threading.Tasks;
using Validation.Phone;

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

        public async Task<IActionResult> PhoneNumberUpdate(UpdatePhoneNumber body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string userId = httpContext.Items["UserID"].ToString();
                PhoneValidator phoneValidator = new PhoneValidator();
                if (phoneValidator.IsPhoneNumberValid(body.PhoneNumber))
                {
                    await this._userRepository.ChangePhoneNumberByUserID(userId, body.PhoneNumber);
                    result = new OkObjectResult(new { message = "You have successfully change your phone number for 2FA" });
                }
                else
                {
                    result = new BadRequestObjectResult(new { error = "You did not provide a valid phone number" });
                }
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "There was an error on our end." });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }

        public async Task<IActionResult> TurnOff2FA(HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string userId = httpContext.Items["UserID"].ToString();
                await this._userRepository.ChangePhone2FAStatusToDisabled(userId);
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "There was an error on our end." });
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
                await this._userRepository.ChangePhone2FAStatusToEnabled(userId);
            }
            catch (Exception ex)
            {
                result = new BadRequestObjectResult(new { error = "There was an error on our end." });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
    }
}
