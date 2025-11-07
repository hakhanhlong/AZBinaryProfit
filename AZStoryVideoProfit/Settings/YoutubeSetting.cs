using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Settings
{
    public class YoutubeSetting
    {
        private static YoutubeSetting _instance;
        private static object synclock = new object();

        protected YoutubeSetting()
        {
            PathSetting = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Json";
            LoadConfiguration();
        }

        public static YoutubeSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        _instance = new YoutubeSetting();
                    }


                }
                return _instance;
            }
        }

        public string PathSetting { get; set; }

        private void LoadConfiguration()
        {

            //Read file to string
            string json = File.ReadAllText($"{PathSetting}\\Youtube.json");
            //Deserialize from file to object:
            var rootObject = new YoutubeSettingViewModel();
            JsonConvert.PopulateObject(json, rootObject);
            Data = rootObject;

        }

        public YoutubeSettingViewModel Data { get; set; }

        public void Save()
        {
            using (StreamWriter file = File.CreateText($"{PathSetting}\\Youtube.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, Data);
            }
        }

    }

    public class YoutubeSettingViewModel
    {
        public YoutubeGenerateTitleViewModel GenerateTitle { get; set; }

        public YoutubeGenerateDescriptionViewModel GenerateDescription { get; set; }

        public YoutubeGenerateThumbnailViewModel GenerateThumbnail {  get; set; }

        public YoutubeGenerateShortScriptViewModel GenerateShortScript { get; set; }

    }

    public class YoutubeGenerateShortScriptViewModel
    {

        public List<YoutubeGenerateShortScript_BaseViewModel> HookTypes { get; set; }
        public List<YoutubeGenerateShortScript_BaseViewModel> HookInstructions { get; set; }
        public List<YoutubeGenerateShortScript_BaseViewModel> ContentTypes { get; set; }

        public List<YoutubeGenerateShortScript_BaseViewModel> ToneStyles { get; set; }

        

    }

    public class YoutubeGenerateTitleViewModel
    {

        public List<YoutubeGenerateTitle_BaseViewModel> ToneStyles {  get; set; }
        public List<YoutubeGenerateTitle_BaseViewModel> TargetAudiences { get; set; }
        public List<YoutubeGenerateTitle_BaseViewModel> UseCases { get; set; }

    }


    public class YoutubeGenerateShortScript_BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class YoutubeGenerateTitle_BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class YoutubeGenerateDescriptionViewModel
    {

        public List<YoutubeGenerateDescription_BaseViewModel> ToneStyles { get; set; }
        public List<YoutubeGenerateDescription_BaseViewModel> TargetAudiences { get; set; }
        public List<YoutubeGenerateDescription_BaseViewModel> UseCases { get; set; }
        public List<YoutubeGenerateDescription_BaseViewModel> SeoGoals { get; set; }

    }

    public class YoutubeGenerateDescription_BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class YoutubeGenerateThumbnailViewModel
    {

        public List<YoutubeGenerateThumbnail_BaseViewModel> TargetAudiences { get; set; }
        public List<YoutubeGenerateThumbnail_BaseViewModel> ContentTypes { get; set; }
        public List<YoutubeGenerateThumbnail_BaseViewModel> StylePreferences { get; set; }
        public List<YoutubeGenerateThumbnail_BaseViewModel> AspectRatios { get; set; }

        public List<YoutubeGenerateThumbnail_BaseViewModel> TextStyles { get; set; }
        public List<YoutubeGenerateThumbnail_BaseViewModel> ImageStyles { get; set; }
        public List<YoutubeGenerateThumbnail_BaseViewModel> ImageFocus { get; set; }        
        

    }

    public class YoutubeGenerateThumbnail_BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


}
