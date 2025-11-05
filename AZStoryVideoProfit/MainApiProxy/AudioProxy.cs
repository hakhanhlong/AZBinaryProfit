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
    public class AudioProxy: IAudioProxy
    {
        private static AudioProxy _instance;
        private static object synclock = new object();
        public AudioProxy() { }

        public static AudioProxy Instance
        {
            get
            {
                lock (synclock)
                {
                    if (_instance == null)
                    {
                        _instance = new AudioProxy();

                    }
                }

                return _instance;
            }
        }

        public AudioScriptResponseViewModel AudioScript(AudioScriptRequestViewModel requestViewModel)
        {
            var client = new RestClient($"{ConfigurationManager.AppSettings["AZBinaryProfit_MainApi_URL"]}");
            var request = new RestRequest($"Audio/AudioScript", Method.Post);
            request.AddParameter("application/json", JsonConvert.SerializeObject(requestViewModel), ParameterType.RequestBody);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<AudioScriptResponseViewModel>(response.Content);
        }
    }
}
