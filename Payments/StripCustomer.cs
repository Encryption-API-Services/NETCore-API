using DataLayer.Mongo.Entities;
using Stripe;
using System;
using System.Threading.Tasks;

namespace Payments
{
    public class StripCustomer
    {
        public StripCustomer()
        {

        }
        public async Task CreateStripCustomer(User user)
        {
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripApiKey");
            CustomerCreateOptions options = new CustomerCreateOptions
            {
                Description = String.Format("The user has the username {0} in the Encryption API Services data", user.Username),
                Email = user.Email
            };
            CustomerService customerService = new CustomerService();
            Customer customer = await customerService.CreateAsync(options);
        }
    }
}