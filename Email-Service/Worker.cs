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
                await GetUsersToSendOutTokens();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task GetUsersToSendOutTokens()
        {
            UserRepository repo = new UserRepository(this._databaseSettings);
            List<User> usersToSendTokens = await repo.GetUsersMadeWithinLastThirtyMinutes();
            foreach (User user in usersToSendTokens)
            {
                await this.GenerateTokenAndSendOut(user);
            }
        }

        private async Task GenerateTokenAndSendOut(User user)
        {
            UserRepository repo = new UserRepository(this._databaseSettings);
            string guid = Guid.NewGuid().ToString();
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            var pub = RSAalg.ToXmlString(false);
            var pubAndPrivate = RSAalg.ToXmlString(true);
            await repo.UpdateUsersRsaKeyPairs(user, pub, pubAndPrivate);


            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;


                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("support@encryptionapiservices.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Hello World";
                    mail.Body = String.Format("<a href='https://localhost:4200/activate/id={0}'>Click here</a>", guid);
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("mulchronemike0191@gmail.com", "cdivtsqzejvpguog");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
