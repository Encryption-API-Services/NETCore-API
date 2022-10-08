using API.ControllersLogic;
using Microsoft.Extensions.DependencyInjection;

namespace UsersAPI.Config
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
            SetupTransient();
            SetupSingleton();
            SetupTransient();
        }
        private void SetupTransient()
        {

        }
        private void SetupSingleton()
        {

        }
        private void SetupScoped()
        {
            this._services.AddScoped<IUserRegisterControllerLogic, UserRegisterControllerLogic>();
        }
    }
}
