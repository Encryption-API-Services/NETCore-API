﻿using Microsoft.AspNetCore.Mvc;
using Models.Encryption;
using System.Threading.Tasks;
using UsersAPI.ControllersLogic;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : Controller
    {
        private readonly IEncryptionControllerLogic _encryptionControllerLogic;
        public EncryptionController(IEncryptionControllerLogic controllerLogic)
        {
            this._encryptionControllerLogic = controllerLogic;
        }

        [HttpPost]
        [Route("EncryptAES")]
        public async Task<IActionResult> EncryptAES([FromBody] EncryptAESRequest body)
        {
            return await this._encryptionControllerLogic.EncryptAES(body, HttpContext);
        }

        [HttpPost]
        [Route("DecryptAES")]
        public async Task<IActionResult> DecryptAES([FromBody] DecryptAESRequest body)
        {
            return await this._encryptionControllerLogic.DecryptAES(body, HttpContext);
        }

        [HttpPost]
        [Route("EncryptSHA1")]
        public async Task<IActionResult> EncryptSHA1([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA1(body, HttpContext);
        }

        [HttpPost]
        [Route("EncryptSHA256")]
        public async Task<IActionResult> EncryptSHA256([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA256(body, HttpContext);
        }

        [HttpPost]
        [Route("EncryptSHA512")]
        public async Task<IActionResult> EncryptSHA512([FromBody] EncryptSHARequest body)
        {
            return await this._encryptionControllerLogic.EncryptSHA512(body, HttpContext);
        }
    }
}