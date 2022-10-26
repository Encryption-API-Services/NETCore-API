using System;
using System.Collections.Generic;
using System.Text;

namespace Encryption.Ciphers
{
    public class CaesarsCipher
    {
        private string[] alaphet = new string[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m","n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private int alaphetLength { get; set; }
        public CaesarsCipher()
        {
            this.alaphetLength = alaphet.Length;
        }
    }
}
