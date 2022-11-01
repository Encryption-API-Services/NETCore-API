using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Validation.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDatabaseSettings _settings;

        public RequestLoggingMiddleware(RequestDelegate next, IDatabaseSettings databaseSettings)
        {
            _next = next;
            this._settings = databaseSettings;
        }
        public async Task Invoke(HttpContext context)
        {
            Guid requestId = Guid.NewGuid();
            LogRequestRepository repo = new LogRequestRepository(this._settings);
            LogRequest requestStart = new LogRequest()
            {
                IsStart = true,
                RequestId = requestId,
                IP = context.Connection.RemoteIpAddress,
                Token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(),
                Method = context.Request.Method,
                CreateTime = DateTime.UtcNow
            };
            await repo.InsertRequest(requestStart);
            await _next(context);
            LogRequest requestEnd = new LogRequest()
            {
                IsStart = false,
                RequestId = requestId,
                IP = context.Connection.RemoteIpAddress,
                Token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last(),
                Method = context.Request.Method,
                CreateTime = DateTime.UtcNow
            };
            await repo.InsertRequest(requestEnd);
        }
    }
}
