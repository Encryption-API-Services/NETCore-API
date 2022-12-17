using Common;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.Credit;
using Payments;
using Stripe;
using System.Reflection;
using Validation.CreditCard;

namespace API.ControllersLogic
{
    public class CreditControllerLogic : ICreditControllerLogic
    {
        private readonly ICreditRepository _creditRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMethodBenchmarkRepository _methodBenchmarkRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEASExceptionRepository _exceptionRepository;
        public CreditControllerLogic(
            ICreditRepository creditRepository,
            IHttpContextAccessor contextAccessor,
            IMethodBenchmarkRepository methodBenchmarkRepository,
            IUserRepository userRepository,
            IEASExceptionRepository exceptionRepository
            )
        {
            this._creditRepository = creditRepository;
            this._contextAccessor = contextAccessor;
            this._methodBenchmarkRepository = methodBenchmarkRepository;
            this._userRepository = userRepository;
            this._exceptionRepository = exceptionRepository;
        }

        #region AddCreditCard
        public async Task<IActionResult> AddCreditCard(AddCreditCardRequest body, HttpContext httpContext)
        {
            BenchmarkMethodLogger logger = new BenchmarkMethodLogger(httpContext);
            IActionResult result = null;
            try
            {
                string userId = httpContext.Items["UserID"].ToString();
                User dbUser = await this._userRepository.GetUserById(userId);
                LuhnWrapper wrapper = new LuhnWrapper();
                if (wrapper.IsCCValid(body.creditCardNumber))
                {
                    StripTokenCard stripTokenCards = new StripTokenCard();
                    // delete card from strip if one exists.
                    if (dbUser.StripCardId != null && dbUser.StripCustomerId != null)
                    {
                        await stripTokenCards.DeleteCustomerCard(dbUser.StripCustomerId, dbUser.StripCardId);
                    }
                    string tokenId = await stripTokenCards.CreateTokenCard(body.creditCardNumber, body.expirationMonth, body.expirationYear, body.SecurityCode);
                    Card newCard = await stripTokenCards.AddTokenCardToCustomer(dbUser.StripCustomerId, tokenId);
                    await this._userRepository.AddCardToUser(dbUser.Id, newCard.Id);
                    result = new OkResult();
                }
            }
            catch (Exception ex)
            {
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestObjectResult(new { error = "There was an error on our end." });
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion

        #region ValidateCreditCard
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
                await this._exceptionRepository.InsertException(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                result = new BadRequestResult();
            }
            logger.EndExecution();
            await this._methodBenchmarkRepository.InsertBenchmark(logger);
            return result;
        }
        #endregion
    }
}