using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace BotNetSpDemo.WebCrawler
{
    public class McDonaldsBrSite
    {
        private const string SITE_URL = @"http://www.mcdonalds.com.br";

        public List<ItemMenu> GetMenu()
        {
            List<ItemMenu> list = new List<ItemMenu>();

            list.AddRange(GetChickenAndFishMenu());
            list.AddRange(GetMeatMenu());

            return list;
        }


        internal List<ItemMenu> GetChickenAndFishMenu()
        {                                                
            List<ItemMenu> list = new List<ItemMenu>();

            string Url = $"{SITE_URL}/cardapio/sanduiches-de-frango-e-peixe";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            var nodes = doc.DocumentNode.SelectNodes("//main/div[2]/div/div");

            foreach (var node in nodes)
            {
                var item = node.SelectSingleNode("a");
                var tagImg = item.SelectSingleNode("div[1]/div[1]/img[1]").Attributes["src"].Value;
                var tagName = item.SelectSingleNode("div[2]").InnerText;

                list.Add(new ItemMenu
                {
                    ItemName = tagName,
                    ImgUrl = tagImg
                });
            }

            return list;
        }

        internal List<ItemMenu> GetMeatMenu()
        {
            List<ItemMenu> list = new List<ItemMenu>();

            string Url = $"{SITE_URL}/cardapio/sanduiches-de-carne";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            var nodes = doc.DocumentNode.SelectNodes("//main/div[2]/div/div");

            foreach (var node in nodes)
            {
                var item = node.SelectSingleNode("a"); 
                var tagImg = item.SelectSingleNode("div[1]/div[1]/img[1]").Attributes["src"].Value;
                var tagName = item.SelectSingleNode("div[2]").InnerText;

                list.Add(new ItemMenu
                {
                    ItemName = tagName,
                    ImgUrl = tagImg
                });
            }

            return list;
        }

        public List<ItemMenu> GetGarnish()
        {
            List<ItemMenu> list = new List<ItemMenu>();

            string Url = $"{SITE_URL}/cardapio/acompanhamentos";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            var nodes = doc.DocumentNode.SelectNodes("//main/div[2]/div/div");

            foreach (var node in nodes)
            {
                var item = node.SelectSingleNode("a");
                var tagImg = item.SelectSingleNode("div[1]/div[1]/img[1]").Attributes["src"].Value;
                var tagName = item.SelectSingleNode("div[2]").InnerText;

                list.Add(new ItemMenu
                {
                    ItemName = tagName,
                    ImgUrl = tagImg
                });
            }

            return list;
        }

        public List<ItemMenu> GetDrink()
        {
            List<ItemMenu> list = new List<ItemMenu>();

            string Url = $"{SITE_URL}/cardapio/bebidas";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);

            var nodes = doc.DocumentNode.SelectNodes("//main/div[2]/div/div");

            foreach (var node in nodes)
            {
                var item = node.SelectSingleNode("a");
                var tagImg = item.SelectSingleNode("div[1]/div[1]/img[1]").Attributes["src"].Value;
                var tagName = item.SelectSingleNode("div[2]").InnerText;

                list.Add(new ItemMenu
                {
                    ItemName = tagName,
                    ImgUrl = tagImg
                });
            }

            return list;
        }
    }


    [Serializable]
    public class ItemMenu
    {
        public string ItemName { get; set; }
        public string ImgUrl { get; set; }
    }
}
