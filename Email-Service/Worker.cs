using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Web;
using System.ComponentModel.Design;

namespace Email_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseSettings _databaseSettings;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            this._databaseSettings = new DatabaseSettings()
            {
                Connection = Environment.GetEnvironmentVariable("Connection"),
                DatabaseName = Environment.GetEnvironmentVariable("DatabaseName"),
                UserCollectionName = Environment.GetEnvironmentVariable("UserCollectionName")
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this._logger.LogInformation("EAS-Email-Service running at: {time}", DateTimeOffset.Now);
                ActivateUser activeUsers = new ActivateUser(this._databaseSettings);
                ForgotPassword forgotPassword = new ForgotPassword(this._databaseSettings);
                LockedOutUsers lockedOutUsers = new LockedOutUsers(this._databaseSettings);
                CCInfoChanged creditCardInfoChanged = new CCInfoChanged(this._databaseSettings);
                BlogPostNewsletter blogPostNewsletter = new BlogPostNewsletter(this._databaseSettings);
                await Task.WhenAll(
                    activeUsers.GetUsersToActivateSendOutTokens(),
                    forgotPassword.GetUsersWhoNeedToResetPassword(),
                    lockedOutUsers.GetUsersThatLockedOut(),
                    creditCardInfoChanged.GetUsersWhoChangedEmailInfo(),
                    blogPostNewsletter.SendNewslettersForBlogPosts()
                );
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
