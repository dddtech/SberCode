using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParserLibrary.Core
{
    class HtmlLoader
    {
        readonly RestClient client;
        readonly string url;
        readonly IEnumerable<KeyValuePair<string, string>> postData;
        public HtmlLoader(IParserSettings settings)
        {
            url = settings.BaseUrl;
            client = new RestClient(url);
            client.Timeout = -1;
            postData = settings.PostData;
        }

        public async Task<string> GetSourcePost()
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cookie", "NID=204=bgBHoNkNeFIMHlj7l041Vf1Jsj9dpw4vRR7b94MSbn9ZuwmIkZkvbfw7Rx2oxqSbPnWBU3zqPYvFhzBqj9_jC-JbA9aQxxct7jgOdGNMGBj6yae6m9TDjQSFQjv70_hartsoGr-2viHO3aw1IYJyZlASXS8ToLzTfkZAKN5K7dU");

            foreach (var item in postData)
            {
                request.AddParameter(item.Key, item.Value);
            }
            IRestResponse response = client.Execute(request);

            return response.Content;
        }
    }
}
