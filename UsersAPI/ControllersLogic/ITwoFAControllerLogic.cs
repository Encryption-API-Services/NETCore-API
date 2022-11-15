﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface ITwoFAControllerLogic
    {
        public Task<IActionResult> TurnOn2FA(HttpContext httpContext);
    }
}
