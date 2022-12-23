using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Encryption
{
    public class SCryptVerifyRequest
    {
        public string hashedPassword { get; set; }
        public string password { get; set; }
    }
}