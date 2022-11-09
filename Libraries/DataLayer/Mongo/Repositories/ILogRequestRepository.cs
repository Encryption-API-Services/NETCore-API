using DataLayer.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface ILogRequestRepository
    {
        public Task<List<LogRequest>> GetTop10RequestsByIP(string ip);
        Task InsertRequest(LogRequest request);
    }
}
