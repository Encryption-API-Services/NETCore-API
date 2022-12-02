using Encryption;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Twilio.TwiML.Messaging;

namespace Common.UniqueIdentifiers
{
    public class Generator
    {
        public Generator()
        {

        }

        public string CreateApiKey()
        {
            string id = Guid.NewGuid().ToString();
            using (HashAlgorithm sha = new ManagedSHAFactory().Get(SHATypes.SHA512))
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(id));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}