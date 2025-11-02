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
    public class StoryVideoProxy: IStoryVideoProxy
    {
        private static StoryVideoProxy _instance;
        private static object synclock = new object();
        public StoryVideoProxy() { }

        public static StoryVideoProxy Instance
        {
            get
            {
                lock (synclock)
                {
                    if (_instance == null)
                    {
                        _instance = new StoryVideoProxy();

                    }
                }

                return _instance;
            }
        }


        public StoryVideoResponseViewModel StoryVideoSceneGenerate(StoryVideoSceneRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"StoryVideo/StoryVideoSceneGenerate", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<StoryVideoResponseViewModel>(response.Content);
        }
    }
}
