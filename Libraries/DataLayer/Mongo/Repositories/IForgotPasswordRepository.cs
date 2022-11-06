﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mongo.Repositories
{
    public interface IForgotPasswordRepository
    {
        public Task<List<string>> GetLastFivePassword(string userId);
    }
}
