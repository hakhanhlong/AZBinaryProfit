using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Settings
{
    public class StoryVideoSetting
    {
        private static StoryVideoSetting _instance;
        private static object synclock = new object();

        protected StoryVideoSetting()
        {
            PathSetting = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Json";
            LoadConfiguration();
        }

        public static StoryVideoSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        _instance = new StoryVideoSetting();
                    }


                }
                return _instance;
            }
        }

        public string PathSetting { get; set; }

        private void LoadConfiguration()
        {

            //Read file to string
            string json = File.ReadAllText($"{PathSetting}\\StoryVideo.json");
            //Deserialize from file to object:
            var rootObject = new StoryVideoSettingViewModel();
            JsonConvert.PopulateObject(json, rootObject);
            Data = rootObject;

        }

        public StoryVideoSettingViewModel Data { get; set; }

        public void Save()
        {
            using (StreamWriter file = File.CreateText($"{PathSetting}\\StoryVideo.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, Data);
            }
        }

    }

    public class StoryVideoSettingViewModel
    {
        public List<StoryVideoSettingItemViewModel> StoryVideoStyles {  get; set; }
        public List<StoryVideoSettingItemViewModel> StoryVideoAspectRatios { get; set; }

    }

    public class StoryVideoSettingItemViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
