using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SteamMarketAgent
{
    class Program
    {
        static System.Timers.Timer timer1 = new System.Timers.Timer();
        public static string urlString;
        public static string desiredPrice;
        static EmailSender mailsender = new EmailSender();
        public static string emailTo;

        static void Main(String[] args)
        {
            var buildManager = new MyBuildManager();
            getEmail();
            getUrlAndPrice();
            buildManager.AddAgent("Agent1", 20, urlString, desiredPrice, emailTo);
            getEmail();
            getUrlAndPrice();

            buildManager.AddAgent("Agent2", 10, urlString, desiredPrice, emailTo);
            
            //Console.Write("Press any key to stop agent...");
            Console.ReadKey();

            buildManager.RemoveAgent("Agent1");
            
            Console.Read();
            Console.WriteLine("Agents canceled");
        }

        public static void getUrlAndPrice()
        {
            Console.Write("Enter link: ");
            urlString = Console.ReadLine();

            Console.Write("Enter desired price ($): ");
            desiredPrice = Console.ReadLine();

            

        }
        public static void getEmail() 
        {
            Console.Write("Email to be notified: ");
            emailTo = Console.ReadLine();
        }

    }
}
