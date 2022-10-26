﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;

namespace UsersAPI.ControllersLogic
{
    public interface IEncryptionControllerLogic
    {
        public Task<IActionResult> EncryptAES(EncryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> DecryptAES(DecryptAESRequest body, HttpContext httpContext);
        public Task<IActionResult> EncryptSHA1(EncryptSHARequest body, HttpContext httpContext);
        public Task<IActionResult> EncryptSHA256(EncryptSHARequest body, HttpContext httpContext);
    }
}
