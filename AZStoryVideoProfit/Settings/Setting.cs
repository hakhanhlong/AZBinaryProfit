using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Settings
{
    public class Setting
    {
        private static Setting _instance;
        private static object synclock = new object();

        protected Setting()
        {
            PathSetting = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Json";
            LoadConfiguration();
        }

        public static Setting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        _instance = new Setting();
                    }


                }
                return _instance;
            }
        }

        public string PathSetting { get; set; }

        private void LoadConfiguration()
        {

            //Read file to string
            string json = File.ReadAllText($"{PathSetting}\\Setting.json");
            //Deserialize from file to object:
            var rootObject = new SettingViewModel();
            JsonConvert.PopulateObject(json, rootObject);
            Data = rootObject;

        }

        public SettingViewModel Data { get; set; }

        public void Save()
        {
            using (StreamWriter file = File.CreateText($"{PathSetting}\\Setting.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, Data);
            }
        }

    }

    public class SettingViewModel
    {
        public string RootAudioOutputPath {  get; set; }
        public string RootShortStoryVideoOutputPath { get; set; }


    }

    public class SettingItemViewModel
    {
        public string RootAudioOutputPath { get; set; }        
        
    }
}
