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
            message.To.InsertOnSubmit(new EmailRecipient(To));
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
            message.From = new MailAddress("robot@commutecarsharing.ru", "CCS Robot");
            message.Subject = Subject;
            message.Body = body;

            var smtp = new SmtpClient();
            var hostname = System.Environment.GetEnvironmentVariable("COMPUTERNAME");

            if (hostname != "DEVSERVER")
            {
                //smtp = new SmtpClient("smtp.yandex.ru", 587);
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("robot@commutecarsharing.ru", "C0mpleXP@$$w0rd");
                smtp = new SmtpClient("in-v3.mailjet.com", 587);
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("d7fafd18a79749b8683f2ee22552d27e", "cd80e0e132624407a2e80f4bd5051523");
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
