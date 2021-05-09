﻿using System;
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
        static void Main(String[] args)
        {
            GetHtmlAsyns();
            Console.Read();
        }

        private static async void GetHtmlAsyns()
        {
            var url = "https://steamcommunity.com/market/listings/730/Desert%20Eagle%20%7C%20Printstream%20%28Minimal%20Wear%29";

            var httpClient = new HttpClient();
            var html =  await httpClient.GetStringAsync(url);

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
                System.Console.Out.WriteLine(skin.SkinModel + "-" + skin.Price);
            }
        }
    }
}
