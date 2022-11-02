using System.Security.Cryptography;

namespace Encryption
{
    public enum SHATypes
    {
        SHA1,
        SHA256,
        SHA512
    }

    public class ManagedSHAFactory
    {

        public HashAlgorithm Get(SHATypes type)
        {
            switch(type)
            {
                case SHATypes.SHA1:
                    return new SHA1Managed();
                case SHATypes.SHA256:
                    return new SHA256Managed();
                case SHATypes.SHA512:
                    return new SHA512Managed();
                default:
                    return null;
            }
        }
    }
}