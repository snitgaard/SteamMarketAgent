using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net.Http;
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
    class BuildAgent
    {
        public string Id { get; private set; }

        public int TimeIntervalSeconds { get; set; }

        public string UrlString { get; set; }
        public string DesiredPrice { get; set; }

        public string EmailTo { get; set; }

        private readonly Thread _t;
        private bool _done;
        static EmailSender mailsender = new EmailSender();


        public BuildAgent(string id, int seconds, string urlString, string desiredPrice, string emailTo)
        {
            Id = id;
            TimeIntervalSeconds = seconds;
            _done = false;
            UrlString = urlString;
            DesiredPrice = desiredPrice;
            EmailTo = emailTo;
            _t = new Thread(Run);
            _t.Start();
        }

        public void Run()
        {
            while (!_done)
            {
                try
                {
                    GetHtmlAsync();

                    Thread.Sleep(TimeIntervalSeconds * 10000);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread interrupted");
                    // just wake up
                }
            }
            Console.WriteLine("Agent stopped");
        }

        public async void GetHtmlAsync()
        {
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter();


                //var url = "https://steamcommunity.com/market/listings/730/Desert%20Eagle%20%7C%20Printstream%20%28Minimal%20Wear%29";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(UrlString);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var producthtml = htmlDocument.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("id", "")
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
                    if (convertedPrice <= Double.Parse(DesiredPrice) && !convertedPrice.Equals(0))
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
            catch 
            {
            }
        }

        public void Cancel()
        {
            _done = true;
            if (_t.ThreadState != System.Threading.ThreadState.Running)
                _t.Interrupt();
        }
    }
}

