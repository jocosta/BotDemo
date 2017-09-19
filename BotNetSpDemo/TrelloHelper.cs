using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace BotNetSpDemo
{
    public class TrelloHelper
    {

        private const string URL = @"https://api.trello.com/1/cards?key={0}&token={1}&name={2}&desc={2}&pos=bottom&dueComplete=false&idList={3}&idLabels=";


        public static void PublishIntoTrello(params string[] itens)
        {
            StringBuilder urlRequest = new StringBuilder();

            var apiKey = WebConfigurationManager.AppSettings["apiKey"];
            var appSecret = WebConfigurationManager.AppSettings["appSecret"];
            var appList = WebConfigurationManager.AppSettings["appList"];

            urlRequest.AppendFormat(URL, apiKey, appSecret, string.Join(" - ", itens), appList);

            var client = new RestClient(urlRequest.ToString());
            var request = new RestRequest(Method.POST);      
            IRestResponse response = client.Execute(request);
        }
    }
}