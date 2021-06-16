using System;
using System.Threading;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
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
        static EmailSender MailSender = new EmailSender();

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

                    Thread.Sleep(TimeIntervalSeconds * 1000);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread interrupted");
                }
            }
            Console.WriteLine("Agent stopped");
        }

        public async void GetHtmlAsync()
        {
            try
            {
                CurrencyConverter currencyConverter = new CurrencyConverter();
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
                        Console.WriteLine("Desired price!");
                        System.Console.Out.WriteLine(skin.SkinModel + " - " + "$" + convertedPrice + " " + skin.Price);
                        MailSender.MailSender();
                        Cancel();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("No skins with desired price found...");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error", e);
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

