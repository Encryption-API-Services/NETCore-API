using System.Threading.Tasks;

namespace Encryption
{
    public class BcryptWrapper
    {
        public string HashPassword(string passwordToHash)
        {

            return BCrypt.Net.BCrypt.HashPassword(passwordToHash);

        }
        public async Task<string> HashPasswordAsync(string passwordToHash)
        {
            return await Task.Run(() =>
            {
                return HashPassword(passwordToHash);
            });
        }
    }
}