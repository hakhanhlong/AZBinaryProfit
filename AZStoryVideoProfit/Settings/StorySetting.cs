using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Settings
{
    public class StorySetting
    {
        private static StorySetting _instance;
        private static object synclock = new object();

        protected StorySetting()
        {
            PathSetting = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Json";
            LoadConfiguration();
        }

        public static StorySetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        _instance = new StorySetting();
                    }


                }
                return _instance;
            }
        }

        public string PathSetting { get; set; }

        private void LoadConfiguration()
        {

            //Read file to string
            string json = File.ReadAllText($"{PathSetting}\\Story.json");
            //Deserialize from file to object:
            var rootObject = new StorySettingViewModel();
            JsonConvert.PopulateObject(json, rootObject);
            Data = rootObject;

        }

        public StorySettingViewModel Data { get; set; }

        public void Save()
        {
            using (StreamWriter file = File.CreateText($"{PathSetting}\\Story.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, Data);
            }
        }

    }

    public class StorySettingViewModel
    {
        public List<StorySettingPersonaViewModel> Personas {  get; set; }

    }

    public class StorySettingPersonaViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
