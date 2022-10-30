using DataLayer.Mongo;
using DataLayer.Mongo.Entities;
using DataLayer.Mongo.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Email_Service
{
    public class ForgotPassword
    {
        private readonly IDatabaseSettings _databaseSettings;

        public ForgotPassword(IDatabaseSettings databaseSettings)
        {
            this._databaseSettings = databaseSettings;
        }
        public async Task GetUsersWhoNeedToResetPassword()
        {
            UserRepository userRepo = new UserRepository(this._databaseSettings);
            List<User> users = await userRepo.GetUsersWhoForgotPassword();
            if (users.Count > 0)
            {
                await this.SendOutForgotEmails(users);
            }
        }

        private async Task SendOutForgotEmails(List<User> users)
        {
            foreach(User user in users)
            {
                byte[] guidBytes = Encoding.UTF8.GetBytes(user.ForgotPassword.Token);
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider(4096);
                var pub = RSAalg.ToXmlString(false);
                var pubAndPrivate = RSAalg.ToXmlString(true);
                byte[] guidBytesSigned = RSAalg.SignData(guidBytes, SHA512.Create());
                try
                {
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    SmtpServer.Port = 587;


                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("support@encryptionapiservices.com");
                        mail.To.Add(user.Email);
                        mail.Subject = "Hello World";
                        mail.Body = "If you did not ask to reset this password please delete this email.</br>" + String.Format("<a href='http://localhost:4200/forgot-password/reset?id={0}&token={1}'>Click here to reset your password.</a>", user.Id, user.ForgotPassword.Token);
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
                    // TODO: update users forgot password field with the updated keys etc.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
