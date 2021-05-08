using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                "https://steamcommunity.com/market/listings/730/Desert%20Eagle%20%7C%20Printstream%20%28Field-Tested%29";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var divs = htmlDocument.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "")
                .Equals("market_table_value")).ToList();

            foreach (var div in divs)
            {
                var skin = new Skin
                {
                    Price = div.Descendants("span").FirstOrDefault().InnerText,
                };
                skins.Add(skin);
            }
        }
    }
}
