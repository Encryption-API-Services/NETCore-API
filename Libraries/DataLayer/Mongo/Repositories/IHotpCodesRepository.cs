using DataLayer.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IHotpCodesRepository
    {
        public Task<long> GetHighestCounter();
        public Task InsertHotpCode(HotpCode code);
    }
}
