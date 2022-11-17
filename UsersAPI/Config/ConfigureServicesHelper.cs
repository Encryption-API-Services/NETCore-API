using API.ControllersLogic;
using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using DataLayer.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Config
{
    public class ConfigureServicesHelper
    {
        private IServiceCollection _services { get; set; }
        private IConfiguration _configuration { get; set; }

        public ConfigureServicesHelper(ref IServiceCollection services, IConfiguration Configuration)
        {
            this._services = services;
            this._configuration = Configuration;
        }
        public void Setup()
        {
            SetupTransient();
            SetupSingleton();
            SetupScoped();
            SetupSwagger();
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

            // Controller Logic
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
            this._services.AddScoped<IUserLoginControllerLogic, UserLoginControllerLogic>();
            this._services.AddScoped<IEncryptionControllerLogic, EncryptionControllerLogic>();
            this._services.AddScoped<ICreditControllerLogic, CreditControllerLogic>();
            this._services.AddScoped<IPasswordControllerLogic, PasswordControllerLogic>();
            this._services.AddScoped<ITwoFAControllerLogic, TwoFAControllerLogic>();
        }
        private void SetupSwagger()
        {
            this._services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Encryption API Services",
                    Version = "v1",
                    Description = "Trusted and test encryption",
                    Contact = new OpenApiContact
                    {
                        Name = "Mike Mulchrone",
                        Email = string.Empty,
                        Url = new Uri("https://encryptionapiservices.com"),
                    },
                });
            });
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