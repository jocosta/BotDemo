using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace BotNetSpDemo.WebCrawler
{
    public class McDonaldsBrSite
    {
        private const string SITE_URL = @"http://www.mcdonalds.com.br";

        public IEnumerable<ItemMenu> GetMenu()
        {


            return null;
        }

        public List<ItemMenu> GetMeatMenu()
        {
            List<ItemMenu> list = new List<ItemMenu>();

            string Url = $"{SITE_URL}/cardapio/sanduiches-de-carne";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            //var nodes = doc.DocumentNode.ChildNodes[2].ChildNodes[3].SelectNodes("//div[@id=\"list-details-v1\"]/div/div");

           var nodes = doc.DocumentNode.SelectNodes("//main/div[2]/div/div");

            foreach (var node in nodes)
            {

                var item = node.SelectSingleNode("a");

                var tagImg = item.SelectSingleNode("div[1]/div[1]/img[1]").Attributes["src"].Value;
                var tagName = item.SelectSingleNode("div[2]").InnerText;

                list.Add(new ItemMenu
                {
                    SandwicheName = tagName,
                    ImgUrl = tagImg
                });

            }

            return list;
        }
    }


    [Serializable]
    public class ItemMenu
    {
        public string SandwicheName { get; set; }
        public string ImgUrl { get; set; }
    }
}
