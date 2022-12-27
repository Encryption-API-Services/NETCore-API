using API.ControllerLogic;
using API.ControllersLogic;
using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using DataLayer.SignalR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;

namespace API.Config
{
    public class ConfigureServicesHelper
    {
        private IServiceCollection _services { get; set; }

        public ConfigureServicesHelper(IServiceCollection services)
        {
            this._services = services;
        }
        public void Setup()
        {
            this._services.AddHttpContextAccessor();
            SetupTransient();
            SetupSingleton();
            SetupScoped();
            SetupSignalR();
        }
        private void SetupTransient()
        {

        }
        private void SetupSingleton()
        {
            this._services.AddSingleton<IDatabaseSettings, DatabaseSettings>();
            this._services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            this._services.AddSingleton<LogRequestService>();
        }
        private void SetupScoped()
        {
            //  Repositories
            this._services.AddScoped<IUserRepository, UserRepository>();
            this._services.AddScoped<IMethodBenchmarkRepository, MethodBenchmarkRepository>();
            this._services.AddScoped<ICreditRepository, CreditRepository>();
            this._services.AddScoped<IHashedPasswordRepository, HashedPasswordRepository>();
            this._services.AddScoped<IFailedLoginAttemptRepository, FailedLoginAttemptRepository>();
            this._services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
            this._services.AddScoped<ILogRequestRepository, LogRequestRepository>();
            this._services.AddScoped<IHotpCodesRepository, HotpCodesRepository>();
            this._services.AddScoped<ISuccessfulLoginRepository, SuccessfulLoginRepository>();
            this._services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            this._services.AddScoped<IEASExceptionRepository, EASExceptionRepository>();
            this._services.AddScoped<ICreditCardInfoChangedRepository, CreditCardInfoChangedRepository>();
            this._services.AddScoped<IRsaEncryptionRepository, RsaEncryptionRepository>();

            // Controller Logic
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
            this._services.AddScoped<IUserLoginControllerLogic, UserLoginControllerLogic>();
            this._services.AddScoped<IEncryptionControllerLogic, EncryptionControllerLogic>();
            this._services.AddScoped<ICreditControllerLogic, CreditControllerLogic>();
            this._services.AddScoped<IPasswordControllerLogic, PasswordControllerLogic>();
            this._services.AddScoped<ITwoFAControllerLogic, TwoFAControllerLogic>();
            this._services.AddScoped<IUIDataControllerLogic, UIDataControllerLogic>();
            this._services.AddScoped<IBlogPostControllerLogic, BlogControllerLogic>();
            this._services.AddScoped<ITokenControllerLogic, TokenControllerLogic>();
            this._services.AddScoped<IRsaControllerLogic, RsaControllerLogic>();
        }
        private void SetupSignalR()
        {
            this._services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins("http://localhost:3000")
                          .AllowCredentials();
                });
            });

            this._services.AddSignalR(configuration =>
            {
                configuration.EnableDetailedErrors = true;
            });
        }
    }
}