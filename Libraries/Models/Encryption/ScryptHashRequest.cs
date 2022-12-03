using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Encryption
{
    public class ScryptHashRequest
    {
        public string passwordToHash { get; set; }
        public string hashedPassword { get; set; }
    }
}
