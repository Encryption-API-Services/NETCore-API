using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class MD5Wrapper
    {
        /// <summary>
        /// Should take in ASCII bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CreateMD5(byte[] input)
        {
            using MD5 md5 = MD5.Create();
            byte[] hashedBytes = md5.ComputeHash(input);
            StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Should take in ASCII bytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> CreateMD5Async(byte[] input)
        {
            return await Task.Run(() =>
            {
                return this.CreateMD5(input);
            });
        }
    }
}
