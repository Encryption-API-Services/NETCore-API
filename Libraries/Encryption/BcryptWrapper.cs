using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Encryption
{
    public class BcryptWrapper
    {
        [DllImport("PerformantEncryption.dll")]
        private static extern string bcrypt_hash(string passToHash);

        [DllImport("PerformantEncryption.dll")]
        private static extern bool bcrypt_verify(string password, string hash);

        public string HashPassword(string passwordToHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(passwordToHash);
        }
        public string HashPasswordPerformant(string passwordToHash)
        {
            return bcrypt_hash(passwordToHash);
        }
        public async Task<string> HashPasswordAsync(string passwordToHash)
        {
            return await Task.Run(() =>
            {
                return HashPassword(passwordToHash);
            });
        }

        public async Task<string> HashPasswordPerformantAsync(string passwordToHash)
        {
            return await Task.Run(() =>
            {
                return HashPasswordPerformant(passwordToHash);
            });
        }


        public async Task<bool> Verify(string hashedPassword, string unhashed)
        {
            return await Task.Run(() => { 
                return BCrypt.Net.BCrypt.Verify(unhashed, hashedPassword);
            });
        }
        public async Task<bool> VerifyPerformant(string hashedPassword, string unhashed)
        {
            return await Task.Run(() => {
                return bcrypt_verify(unhashed, hashedPassword);
            });
        }
    }
}