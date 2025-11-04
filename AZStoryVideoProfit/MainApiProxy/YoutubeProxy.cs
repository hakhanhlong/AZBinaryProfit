using AZStoryVideoProfit.MainApiProxy.Interfaces;
using AZStoryVideoProfit.MainApiProxy.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy
{
    public class YoutubeProxy: IYoutubeProxy
    {
        private static YoutubeProxy _instance;
        private static object synclock = new object();
        public YoutubeProxy() { }

        public static YoutubeProxy Instance
        {
            get
            {
                lock (synclock)
                {
                    if (_instance == null)
                    {
                        _instance = new YoutubeProxy();

                    }
                }

                return _instance;
            }
        }

        public YoutubeGenerateTitleResponseViewModel GenerateTitle(YoutubeGenerateTitleRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateTitle", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateTitleResponseViewModel>(response.Content);
        }
    }
}
