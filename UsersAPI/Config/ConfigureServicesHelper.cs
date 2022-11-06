using API.ControllersLogic;
using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Reflection.Metadata;
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
            SetupDatabase();
            SetupTransient();
            SetupSingleton();
            SetupScoped();
            SetupSwagger();
        }

        private void SetupDatabase()
        {
            this._services.Configure<DatabaseSettings>(this._configuration.GetSection(nameof(DatabaseSettings)));
        }
        private void SetupTransient()
        {

        }
        private void SetupSingleton()
        {
            this._services.AddSingleton<IDatabaseSettings, DatabaseSettings>(x => x.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            this._services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            // Controller Logic
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
            this._services.AddScoped<IUserLoginControllerLogic, UserLoginControllerLogic>();
            this._services.AddScoped<IEncryptionControllerLogic, EncryptionControllerLogic>();
            this._services.AddScoped<ICreditControllerLogic, CreditControllerLogic>();
            this._services.AddScoped<IPasswordControllerLogic, PasswordControllerLogic>();
        }
        private void SetupSwagger()
        {
            this._services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Encryption API Services";
                    document.Info.Description = "Encryption Service";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Mike Mulchrone",
                        Email = Environment.GetEnvironmentVariable("Email"),
                        Url = "encryptionapiservices.com"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Trademark Encryption API Services",
                        Url = "https://encryptionapiservices.com"
                    };
                };
            });
        }
    }
}