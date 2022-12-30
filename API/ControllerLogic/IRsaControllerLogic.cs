﻿using Microsoft.AspNetCore.Mvc;
using Models.Encryption;

namespace API.ControllerLogic
{
    public interface IRsaControllerLogic
    {
        Task<IActionResult> GetKeyPair(HttpContext context, int keySize);
        Task<IActionResult> EncryptWithoutPublic(HttpContext context, EncryptWithoutPublicRequest body);
        Task<IActionResult> EncryptWithPublic(HttpContext context, EncryptWithPublicRequest body);
        Task<IActionResult> Decrypt(HttpContext context, RsaDecryptRequest body);
        Task<IActionResult> SignWithoutKey(HttpContext context, RsaSignWithoutKeyRequest body);
        Task<IActionResult> Verify(HttpContext context, RsaVerifyRequest body);
        Task<IActionResult> VerifyWithKey(HttpContext context, RsaSignWithKeyRequest body);
    }
}