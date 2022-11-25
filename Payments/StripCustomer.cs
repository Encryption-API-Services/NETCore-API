using System;
using System.Collections.Generic;
using System.Text;

namespace Payments
{
    public class StripCustomer
    {
        private string _apiKey { get; set; }

        public StripCustomer()
        {
            this._apiKey = Environment.GetEnvironmentVariable("StripApiKey");
        }
    }
}
