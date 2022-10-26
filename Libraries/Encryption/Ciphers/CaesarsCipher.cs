using System;
using System.Collections.Generic;
using System.Text;

namespace Encryption.Ciphers
{
    public class CaesarsCipher
    {
        private List<string> alaphet = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m","n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private int alaphetLength { get; set; }
        public CaesarsCipher()
        {
            this.alaphetLength = alaphet.Count;
        }

        public string Encrypt(string text, int shift)
        {
            StringBuilder sb = new StringBuilder();
            // loop through the text
            //check is character is listed in the alaphet
            return sb.ToString();
        }

        public string Decrypt(string text, int shift)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }

        private void Shift(ref string first, ref string second)
        {
            // create a placeholder
            // store first in placeholder
            // store second in first
        }
    }
}
