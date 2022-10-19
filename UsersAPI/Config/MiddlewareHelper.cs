using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace UsersAPI.Config
{
    public class MiddlewareHelper
    {
        private readonly IApplicationBuilder _app;
        public MiddlewareHelper(IApplicationBuilder app)
        {
            this._app = app;
        }
        public void Setup()
        {
            RequestStart();
            RequestEnd();
        }

        /// <summary>
        /// Logs basic information about the beginning of the request
        /// </summary>
        private async void RequestStart()
        {
            this._app.Use(async (context, next) =>
            {
                
            });
        }

        /// <summary>
        /// Logs basic information about the end of the request. 
        /// </summary>
        private async void RequestEnd()
        {
            this._app.Use(async (context, next) =>
            {

            });
        }
    }
}
