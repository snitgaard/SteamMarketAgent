using System;
using System.Threading;

namespace SteamMarketAgent
{
    class Program
    {
        public static string UrlString;
        public static string DesiredPrice;
        public static string EmailTo;

        static void Main(String[] args)
        {
            var buildManager = new MyBuildManager();
            getEmail();
            getUrlAndPrice();
            buildManager.AddAgent("Agent1", 20, UrlString, DesiredPrice, EmailTo);

            Thread.Sleep(5000);

            getEmail();
            getUrlAndPrice();
            buildManager.AddAgent("Agent2", 10, UrlString, DesiredPrice, EmailTo);
            
            Console.Write("Press any key to stop agent...");
            Console.ReadKey();

            buildManager.RemoveAgent("Agent1");
            buildManager.RemoveAgent("Agent2");

            Console.Read();
            Console.WriteLine("Agents canceled");
        }

        public static void getUrlAndPrice()
        {
            Console.Write("Enter link: ");
            UrlString = Console.ReadLine();

            Console.Write("Enter desired price ($): ");
            DesiredPrice = Console.ReadLine();
        }
        public static void getEmail() 
        {
            Console.Write("Email to be notified: ");
            EmailTo = Console.ReadLine();
        }

    }
}
