using Microsoft.AspNet.Identity;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace personal_site_api.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message) {
            await configSendGridAsync(message);
        }
        
        public async Task configSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();

            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("craigbartjr@gmail.com", "Craig Bartholomew");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Username"],
                                                    ConfigurationManager.AppSettings["emailService:Password"]);

            var transportWeb = new Web(credentials);

            if (transportWeb != null)
            {
                //send email
                await transportWeb.DeliverAsync(myMessage);
            } else
            {
                //errors
                await Task.FromResult(0);
            }
        }
    }
}