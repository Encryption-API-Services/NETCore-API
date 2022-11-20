using DataLayer.Mongo.Entities;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface ISuccessfulLoginRepository
    {
        public Task InsertSuccessfulLogin(SuccessfulLogin login);
    }
}
