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


            SendEmail(client, Program.EmailTo);
            Console.WriteLine("E-mail has been sent");
        }
        public void SendEmail(SmtpClient client, string to)
        {
            using (var mail = new MailMessage())
            {
                mail.To.Add(new MailAddress(to));
                mail.From = new MailAddress("steammarketagent@gmail.com", "the cowboys");
                mail.Subject = "You got skins!";
                mail.Body = "Dit valgte skin:" + " " + Program.UrlString + 
                            "<br /> er blevet fundet til følgende pris:" + " " + "$" + Program.DesiredPrice + " " + "eller under." +
                    "<br /> - The Cowboys";
                mail.IsBodyHtml = true;

                client.Send(mail);
            }
        }


    }
}
