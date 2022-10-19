using DataLayer.Mongo.Entities;
using Models.UserAuthentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IUserRepository
    {
        public Task AddUser(RegisterUser model);
        public Task<User> GetUserByEmail(string email);
        public Task<List<User>> GetUsersMadeWithinLastThirtyMinutes();
    }
}
