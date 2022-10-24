using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Middleware
{
    public class ValidateJWToken
    {
        private readonly RequestDelegate _next;
        private readonly IDatabaseSettings _settings;

        public ValidateJWToken(RequestDelegate next, IDatabaseSettings databaseSettings)
        {
            _next = next;
            this._settings = databaseSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await ValidateToken(token);

            await _next(context);
        }
        private async Task ValidateToken(string token)
        {

        }
    }
}
