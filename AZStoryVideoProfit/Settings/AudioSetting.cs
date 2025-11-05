using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Settings
{
    public class AudioSetting
    {
        private static AudioSetting _instance;
        private static object synclock = new object();

        protected AudioSetting()
        {
            PathSetting = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Json";
            LoadConfiguration();
        }

        public static AudioSetting Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        _instance = new AudioSetting();
                    }


                }
                return _instance;
            }
        }

        public string PathSetting { get; set; }

        private void LoadConfiguration()
        {

            //Read file to string
            string json = File.ReadAllText($"{PathSetting}\\Audio.json");
            //Deserialize from file to object:
            var rootObject = new AudioSettingViewModel();
            JsonConvert.PopulateObject(json, rootObject);
            Data = rootObject;

        }

        public AudioSettingViewModel Data { get; set; }

        public void Save()
        {
            using (StreamWriter file = File.CreateText($"{PathSetting}\\Audio.json"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(file, Data);
            }
        }

    }

    public class AudioSettingViewModel
    {
        public List<Audio_BaseViewModel> ContentStyles { get; set; }
        

        public List<Audio_BaseViewModel> TTSStyles { get; set; }

        public List<Audio_BaseViewModel> Languages { get; set; }

        public List<Audio_BaseViewModel> ScriptLenghts { get; set; }

        public List<Audio_BaseViewModel> VoicePersonas { get; set; }

        public List<Audio_BaseViewModel> Styles { get; set; }

        public List<Audio_BaseViewModel> Types { get; set; }


    }

    public class Audio_BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


}
