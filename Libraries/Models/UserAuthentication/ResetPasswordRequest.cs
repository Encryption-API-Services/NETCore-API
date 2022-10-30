using System;
using System.Collections.Generic;
using System.Text;

namespace Models.UserAuthentication
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
