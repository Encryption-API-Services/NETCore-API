using System;
using System.Collections.Generic;
using System.Text;

namespace Models.UserAuthentication
{
    public class ValidateHotpCode
    {
        public string UserId { get; set; }
        public string HotpCode { get; set; }
    }
}
