﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Models.Encryption;
using System;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : Controller
    {
        private readonly IPasswordControllerLogic _passwordControllerLogic;
        public PasswordController(IPasswordControllerLogic passwordControllerLogic)
        {
            this._passwordControllerLogic = passwordControllerLogic;
        }

        [HttpPost]
        [Route("BCryptEncrypt")]
        public async Task<IActionResult> BcryptPassword([FromBody]BCryptEncryptModel body)
        {
            return await this._passwordControllerLogic.BcryptEncryptPassword(body, HttpContext);
        }

        [HttpPost]
        [Route("BcryptVerify")]
        public async Task<IActionResult> BcryptVerifyPassword([FromBody] BcryptVerifyModel body)
        {
            return await this._passwordControllerLogic.BcryptVerifyPassword(body, HttpContext);
        }
    }
}