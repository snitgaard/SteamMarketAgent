using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SteamMarketAgent
{
    public class EmailSender
    {
        public void MailSender()
        {
            using var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("steammarketagent@gmail.com", "xcuyydbwfweztxti"),
                EnableSsl = true
            };
            Console.Write("receiver: ");
            var emailTo = Console.ReadLine();

            SendEmail(client, emailTo);
            Console.WriteLine("E-mail has been send");
        }
        public void SendEmail(SmtpClient client, string to)
        {
            using (var mail = new MailMessage())
            {
                mail.To.Add(new MailAddress(to));
                mail.From = new MailAddress("steammarketagent@gmail.com", "the cowboys");
                mail.Subject = "you got skins";
                mail.Body = "dit skin er under prisen du valgte";
                mail.IsBodyHtml = true;

                client.Send(mail);
            }
        }


    }
}
