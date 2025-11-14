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


        public YoutubeIdeaResponseViewModel YoutubeIdea(YoutubeIdeaRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/YoutubeIdea", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeIdeaResponseViewModel>(response.Content);

        }


        public YoutubeShortVideoScriptResponse YoutubeShortVideoScript(YoutubeShortVideoScriptRequest requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/YoutubeShortVideoScript", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeShortVideoScriptResponse>(response.Content);
        }

        public YoutubeShortVideoScriptNarrationResponse YoutubeShortVideoScriptNarration(YoutubeShortVideoScriptNarrationRequest requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/YoutubeShortVideoScriptNarration", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeShortVideoScriptNarrationResponse>(response.Content);
        }

        public YoutubeGenerateTitleResponseViewModel GenerateTitle(YoutubeGenerateTitleRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateTitle", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateTitleResponseViewModel>(response.Content);
        }

        public YoutubeGenerateDescriptionResponseViewModel GenerateDescription(YoutubeGenerateDescriptionRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateDescription", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateDescriptionResponseViewModel>(response.Content);
        }


        public YoutubeGenerateThumbnail_ThumbnailConceptResponseViewModel GenerateThumbnailConcept(YoutubeGenerateThumbnail_ThumbnailConceptRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateThumbnail_ThumbnailConcepts", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateThumbnail_ThumbnailConceptResponseViewModel>(response.Content);
        }

        public YoutubeGenerateThumbnail_ThumbnailImagePromptResponseViewModel GenerateThumbnail_ImagePrompt(YoutubeGenerateThumbnail_ThumbnailImagePromptRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateThumbnail_ThumbnailImagePrompt", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateThumbnail_ThumbnailImagePromptResponseViewModel>(response.Content);
        }


        public YoutubeGenerateStoryVideoScriptResponseViewModel GenerateStoryVideoScript(YoutubeGenerateStoryVideoScriptRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Youtube/GenerateStoryVideoScript", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<YoutubeGenerateStoryVideoScriptResponseViewModel>(response.Content);
        }
    }
}
