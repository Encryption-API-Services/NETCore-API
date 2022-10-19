using API.ControllersLogic;
using DataLayer.Mongo;
using DataLayer.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        }
        private void SetupScoped()
        {
            this._services.AddScoped<IUserRepository, UserRepository>();
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
        }
    }
}