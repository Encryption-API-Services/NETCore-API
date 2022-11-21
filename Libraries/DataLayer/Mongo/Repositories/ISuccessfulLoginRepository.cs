using DataLayer.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface ISuccessfulLoginRepository
    {
        public Task InsertSuccessfulLogin(SuccessfulLogin login);
        public Task<List<SuccessfulLogin>> GetAllSuccessfulLoginWithinTimeFrame(string userId, DateTime dateTime);
        public Task UpdateSuccessfulLoginWasMe(string loginId, bool wasThisMe);
    }
}
