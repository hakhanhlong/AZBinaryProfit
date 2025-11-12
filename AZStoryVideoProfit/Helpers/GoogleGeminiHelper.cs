using AZStoryVideoProfit.MainApiProxy.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace AZStoryVideoProfit.Helpers
{
    public class GoogleGeminiHelper
    {
        public static string GenerateText2Speech(string chunkText, string voiceName = "Kore", string modelName= "gemini-2.5-flash-preview-tts", 
            string apiKey = "AIzaSyAobELdluKtCD8cN3Md7Mvsg0LibMLBU00")
        {


            var options = new RestClientOptions($"https://generativelanguage.googleapis.com")
            {
                ThrowOnAnyError = true,                
                Timeout = TimeSpan.FromSeconds(900) // 1 second
            };
            var client = new RestClient(options);
            

            var request = new RestRequest($"/v1beta/models/{modelName}:generateContent?key={apiKey}", Method.Post);
            request.AddHeader("Content-Type", "application/json");



            var body = new Dictionary<string, object>
            {
                ["contents"] = new List<object>
                {
                    new {
                            role = "user",
                            parts = new List<object>{
                                new {text = chunkText}
                            }
                    }
                },
                ["safetySettings"] = new List<object>
                {
                    new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE"},
                    new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE"},
                    new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE"},
                    new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE"},
                },
                ["generationConfig"] = new
                {
                    responseModalities = new List<string>
                    {
                        "audio"
                    },
                    temperature = 1,
                    speech_config = new
                    {
                        voice_config = new
                        {
                            prebuilt_voice_config = new
                            {
                                voice_name = "Kore"
                            }
                        }
                    }
                }
            };


            string jsonRequest = JsonConvert.SerializeObject(body);
            request.AddStringBody(jsonRequest, DataFormat.Json);

            var response = client.Execute(request);

            return response.Content;
        }


        public static string GenerateText2Image(string promptText, string modelName = "gemini-2.5-flash-image",
           string apiKey = "AIzaSyAobELdluKtCD8cN3Md7Mvsg0LibMLBU00", string aspectRatio = "16:9")
        {


            var options = new RestClientOptions($"https://generativelanguage.googleapis.com")
            {
                ThrowOnAnyError = true,
                Timeout = TimeSpan.FromSeconds(900) // 1 second
            };
            var client = new RestClient(options);


            var request = new RestRequest($"/v1beta/models/{modelName}:generateContent?key={apiKey}", Method.Post);
            request.AddHeader("Content-Type", "application/json");



            var body = new Dictionary<string, object>
            {
                ["contents"] = new List<object>
                {
                    new {
                            role = "user",
                            parts = new List<object>{
                                new {text = promptText}
                            }
                    }
                },               
                ["generationConfig"] = new
                {
                    responseModalities = new List<string>
                    {
                        "IMAGE",
                        "TEXT"
                    },
                    imageConfig = new
                    {
                        aspectRatio = aspectRatio,
                        image_size = "1K"
                    }
                }
            };


            string jsonRequest = JsonConvert.SerializeObject(body);
            request.AddStringBody(jsonRequest, DataFormat.Json);

            var response = client.Execute(request);

            return response.Content;
        }
    }
}
