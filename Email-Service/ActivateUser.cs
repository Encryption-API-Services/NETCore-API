using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Mongo;
using Encryption;

namespace Email_Service
{
    public class ActivateUser
    {
        private readonly IDatabaseSettings _databaseSettings;
        public ActivateUser(IDatabaseSettings databaseSettings)
        {
            this._databaseSettings = databaseSettings;
        }
        public async Task GetUsersToActivateSendOutTokens()
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
            byte[] guidBytes = Encoding.UTF8.GetBytes(guid);
            RSAProviderWrapper rsa4096 = new RSAProviderWrapper(4096);
            byte[] guidBytesSigned = rsa4096.provider.SignData(guidBytes, SHA512.Create());
            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Port = 587;


                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("support@encryptionapiservices.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Account Activation - Encryption API Services ";
                    mail.Body = "We are excited to have you here </br>" + String.Format("<a href='" + Environment.GetEnvironmentVariable("Domain") + "/#/activate?id={0}&token={1}'>Click here to activate</a>", user.Id, guid);
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        string email = Environment.GetEnvironmentVariable("Email");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(email, "pwqnyquwgjsxhosv");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                await repo.UpdateUsersRsaKeyPairsAndToken(user, rsa4096.publicKey, rsa4096.privateKey, guid, guidBytesSigned);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
