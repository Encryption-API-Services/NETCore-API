using DataLayer.Mongo;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Text_Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDatabaseSettings _databaseSettings;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
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
                TwoFactorAuthHotpCode twoFactorHotpCodes = new TwoFactorAuthHotpCode(this._databaseSettings);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.WhenAll(
                    twoFactorHotpCodes.GetHotpCodesToSendOut()
                    );
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
