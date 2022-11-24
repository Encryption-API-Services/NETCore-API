﻿using API.ControllersLogic;
using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using DataLayer.SignalR;
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

            // Controller Logic
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
            this._services.AddScoped<IUserLoginControllerLogic, UserLoginControllerLogic>();
            this._services.AddScoped<IEncryptionControllerLogic, EncryptionControllerLogic>();
            this._services.AddScoped<ICreditControllerLogic, CreditControllerLogic>();
            this._services.AddScoped<IPasswordControllerLogic, PasswordControllerLogic>();
            this._services.AddScoped<ITwoFAControllerLogic, TwoFAControllerLogic>();
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