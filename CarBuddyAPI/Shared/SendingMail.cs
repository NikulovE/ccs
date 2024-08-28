using System;

#if NETFX_CORE
using Windows.ApplicationModel.Email;
using LightBuzz.SMTP;
#else
using System.Linq;
using System.IO;
using System.Security;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
#endif

namespace Shared
{
    public class SendingMail
    {
#if NETFX_CORE
        public static async void SendEmail(String To, String body, String Subject)
        {
            var smtp = new SmtpClient("smtp.gmail.com", 465);
            smtp.EnableSsl = true;

            var message = new EmailMessage();
            message.To.Add(new EmailRecipient(To));
            message.Sender.Address = "carbuddyu@gmail.com";
            smtp.Username = "carbuddyu";
            smtp.Password = "C0mpleXP@$$w0rd";
            message.Subject = Subject;
            message.Body = body;
            try
            {
                await smtp.Send(message);
            }
            catch (Exception)
            {
            };


        }
#else
        public static bool SendEmail(String To, String body, String Subject)
        {
            var message = new MailMessage();
            message.To.Add(To);
            message.From = new MailAddress("robot@unicarbuddy.ru", "UniCarBuddy Robot");
            message.Subject = Subject;
            message.Body = body;

            var smtp = new SmtpClient();
            var hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

            if (hostname != "LOCALSERVERRU")
            {
                smtp = new SmtpClient("smtp.yandex.ru", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("robot@unicarbuddy.ru", "C0mpleXP@$$w0rd");
            }
            else {
                smtp = new SmtpClient("smtp.testrussia.local");
            }



            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            
            try
            {

                smtp.Send(message);
                message.Dispose();
                smtp.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            };


        }

#endif
    }
}
