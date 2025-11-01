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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AZStoryVideoProfit.MainApiProxy
{
    public class StoryProxy: IStoryProxy
    {
        private static StoryProxy _instance;
        private static object synclock = new object();
        public StoryProxy() { }

        public static StoryProxy Instance
        {
            get
            {
                lock (synclock)
                {
                    if (_instance == null)
                    {
                        _instance = new StoryProxy();

                    }
                }

                return _instance;
            }
        }



        public StoryIdeaResponseViewModel StoryIdea(StoryIdeaRequestViewModel requestViewModel)
        {
            var model = new StoryIdeaRequestViewModel
            {
                IdeaNumber = requestViewModel.IdeaNumber,
                Topic = requestViewModel.Topic,
                Language = requestViewModel.Language
            };
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Story/StoryIdea", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<StoryIdeaResponseViewModel>(response.Content);


        }

        public StorySettingResponseViewModel StorySetting(StorySettingRequestViewModel requestViewModel)
        {
           
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Story/StorySetting", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<StorySettingResponseViewModel>(response.Content);
        }

        public StoryPremiseResponseViewModel StoryPremise(StoryPremiseRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Story/StoryPremise", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<StoryPremiseResponseViewModel>(response.Content);
        }
    }
}
