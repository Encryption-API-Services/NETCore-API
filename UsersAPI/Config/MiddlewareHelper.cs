using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Builder;
using System;

namespace UsersAPI.Config
{
    public class MiddlewareHelper
    {
        private readonly IApplicationBuilder _app;
        private readonly IDatabaseSettings _databaseSettings;
        public MiddlewareHelper(IApplicationBuilder app)
        {
            this._app = app;
        }
        public void Setup()
        {
            RequestStart();
            
        }

        /// <summary>
        /// Logs basic information about the beginning of the request
        /// </summary>
        private async void RequestStart()
        {
            this._app.Use(async (context, next) =>
            {
                LogRequest requestStart = new LogRequest()
                {
                    IsStart = true,
                    IP = context.Connection.RemoteIpAddress,
                    Method = context.Request.Method,
                    CreateTime = DateTime.UtcNow
                };
                LogRequestRepository repo = new LogRequestRepository(this._databaseSettings);
                await repo.InsertRequest(requestStart);
                await next();
                LogRequest requestEnd = new LogRequest()
                {
                    IsStart = false,
                    IP = context.Connection.RemoteIpAddress,
                    Method = context.Request.Method,
                    CreateTime = DateTime.UtcNow
                };
            });
        }
    }
}
