using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using DataLayer.SignalR;
using Microsoft.AspNetCore.Http;

using System;
using System.Linq;
using System.Threading.Tasks;
using Validation.Networking;

namespace Validation.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDatabaseSettings _settings;
        private readonly LogRequestService _logRequestService;

        public RequestLoggingMiddleware(RequestDelegate next, IDatabaseSettings databaseSettings, LogRequestService logRequestService)
        {
            _next = next;
            this._settings = databaseSettings;
            this._logRequestService = logRequestService;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                Guid requestId = Guid.NewGuid();
                LogRequestRepository repo = new LogRequestRepository(this._settings);
                LogRequest requestStart = new LogRequest()
                {
                    IsStart = true,
                    RequestId = requestId,
                    IP = IPAddressExtension.ConvertContextToLocalHostIp(context.Connection.RemoteIpAddress.ToString()),
                    Token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(),
                    Method = context.Request.Method,
                    CreateTime = DateTime.UtcNow
                };
                await this._logRequestService.SendLogRequestToMetricsClient(requestStart);
                await repo.InsertRequest(requestStart);
                await _next(context);
                LogRequest requestEnd = new LogRequest()
                {
                    IsStart = false,
                    RequestId = requestId,
                    IP = IPAddressExtension.ConvertContextToLocalHostIp(context.Connection.RemoteIpAddress.ToString()),
                    Token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(),
                    Method = context.Request.Method,
                    CreateTime = DateTime.UtcNow
                };
                await repo.InsertRequest(requestEnd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
