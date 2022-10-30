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
                Connection = _configuration.GetValue<string>("DatabaseSettings:Connection"),
                DatabaseName = _configuration.GetValue<string>("DatabaseSettings:Databasename"),
                UserCollectionName = _configuration.GetValue<string>("DatabaseSettings:UserCollectionName")
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                ActivateUser activeUsers = new ActivateUser(this._databaseSettings);
                await Task.WhenAll(activeUsers.GetUsersToActivateSendOutTokens());
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
