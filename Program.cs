using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SteamMarketAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerAsync();
            Console.ReadLine();
        }

        private static async Task startCrawlerAsync()
        {
            var skins = new List<Skin>();
            var url =
                "https://steamcommunity.com/market/listings/730/Desert%20Eagle%20%7C%20Printstream%20%28Minimal%20Wear%29";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var divs = htmlDocument.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "")
                .Equals("market_listing_row market_recent_listing_row listing")).ToList();

            foreach (var div in divs)
            {
                var skin = new Skin
                {
                    SkinModel = div.Descendants("span").FirstOrDefault().ChildAttributes("span").FirstOrDefault().Value,
                    Price = div.Descendants("span").FirstOrDefault(node => node.GetAttributeValue("class", "")
                        .Equals("market_listing_price market_listing_price_with_fee")).InnerText,
                };
                skins.Add(skin);
                Console.WriteLine(skin.Price);
            }

        }
    }
}
