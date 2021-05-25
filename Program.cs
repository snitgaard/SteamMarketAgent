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

            InitTimer();
            getUrlAndPrice();
            GetHtmlAsync();
            Console.Read();
        }

        public static void InitTimer()
        { 
            timer1.Interval = 60000;//one minute
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Start();
        }

        static private void timer1_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.Clear();
            GetHtmlAsync();
        }

        public static void getUrlAndPrice()
        {

            Console.Write("Enter link: ");
            urlString = Console.ReadLine();
            
            Console.Write("Enter desired price ($): ");
            desiredPrice = Console.ReadLine();

           
            Console.Write("Email to be notified: ");
            emailTo = Console.ReadLine();

        }

        public static async void GetHtmlAsync()
        {
            CurrencyConverter currencyConverter = new CurrencyConverter();

            var uri = new UriBuilder(urlString).Uri;
            //var url = "https://steamcommunity.com/market/listings/730/Desert%20Eagle%20%7C%20Printstream%20%28Minimal%20Wear%29";
            var httpClient = new HttpClient();
            var html =  await httpClient.GetStringAsync(uri);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var producthtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id","")
                    .Equals("searchResultsRows")).ToList();

            var productListItems = producthtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                    .Contains("listing")).ToList();

            var skins = new List<Skin>();

            //var productList = producthtml[0].Descendants()
            foreach (var productListItem in productListItems)
            {
                var skin = new Skin
                {
                    Id = productListItem.GetAttributeValue("id", ""),
                    SkinModel = productListItem
                        .Descendants("span").FirstOrDefault(node => node.GetAttributeValue("class", "")
                            .Equals("market_listing_item_name")).InnerHtml,
                    Price = productListItem
                        .Descendants("span").FirstOrDefault(node => node.GetAttributeValue("class", "")
                            .Equals("market_listing_price market_listing_price_with_fee")).InnerHtml,
                };
                skins.Add(skin);
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                double convertedPrice = currencyConverter.currencyConversion(skin.Price);
                if (convertedPrice <= Double.Parse(desiredPrice) && !convertedPrice.Equals(0))
                {
                    //Send email
                    Console.WriteLine("Desired price!");
                    System.Console.Out.WriteLine(skin.SkinModel + " - " + "$" + convertedPrice + " " + skin.Price);
                    mailsender.MailSender();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("No skins with desired price found...");
                }
            }
        }
    }
}
