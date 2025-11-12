using AZStoryVideoProfit.Helpers;
using AZStoryVideoProfit.MainApiProxy;
using AZStoryVideoProfit.Settings;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace AZStoryVideoProfit.Forms
{
    public partial class AudioForm : Form
    {
        private List<ChunkTextItem> _ChunkTexts;
        private Hashtable ListViewItemToChunkTextItem = new Hashtable();
        private Hashtable ChunkTextItemToListViewItem = new Hashtable();

        public AudioForm()
        {
            InitializeComponent();
        }

        private void InitData()
        {
            AudioScript_Style.DataSource = AudioSetting.Instance.Data.Styles;
            AudioScript_Style.DisplayMember = "Name";
            AudioScript_Style.ValueMember = "Description";

            AudioScript_VoicePersonas.DataSource = AudioSetting.Instance.Data.VoicePersonas;
            AudioScript_VoicePersonas.DisplayMember = "Name";
            AudioScript_VoicePersonas.ValueMember = "Description";

            AudioScript_ContentStyle.DataSource = AudioSetting.Instance.Data.ContentStyles;
            AudioScript_ContentStyle.DisplayMember = "Name";
            AudioScript_ContentStyle.ValueMember = "Description";

            AudioScript_TTSStyles.DataSource = AudioSetting.Instance.Data.TTSStyles;
            AudioScript_TTSStyles.DisplayMember = "Name";
            AudioScript_TTSStyles.ValueMember = "Description";

            AudioScript_ScriptLength.DataSource = AudioSetting.Instance.Data.ScriptLenghts;
            AudioScript_ScriptLength.DisplayMember = "Name";
            AudioScript_ScriptLength.ValueMember = "Description";


            AudioScript_Language.DataSource = AudioSetting.Instance.Data.Languages;
            AudioScript_Language.DisplayMember = "Name";
            AudioScript_Language.ValueMember = "Description";

            AudioScript_Type.DataSource = AudioSetting.Instance.Data.Types;
            AudioScript_Type.DisplayMember = "Name";
            AudioScript_Type.ValueMember = "Description";



        }

        private void AudioForm_Load(object sender, EventArgs e)
        {

            string test = Setting.Instance.Data.RootAudioOutputPath;
            this.WindowState = FormWindowState.Maximized;
            InitData();

            _ChunkTexts = new List<ChunkTextItem>();
        }

        private void btnAudioScript_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {

                    lvChunkScripts.Items.Clear();
                    txtAudioScript_ScriptText.Text = "";
                    ListViewItemToChunkTextItem.Clear();
                    ChunkTextItemToListViewItem.Clear();
                    _ChunkTexts.Clear();

                    var request = new MainApiProxy.ViewModels.AudioScriptRequestViewModel
                    {
                        Title = txtAudioScript_Title.Text,
                        Story = txtAudioScript_Story.Text,                        
                        NarratorPersona = AudioScript_VoicePersonas.SelectedValue.ToString(),
                        ContentInstruction = AudioScript_ContentStyle.SelectedValue.ToString(),
                        Style = AudioScript_Style.SelectedValue.ToString(),
                        TTSInstruction = AudioScript_TTSStyles.SelectedValue.ToString(),
                        Language = AudioScript_Language.SelectedValue.ToString(),
                        ScriptLength = AudioScript_ScriptLength.SelectedValue.ToString(),
                        Type = AudioScript_Type.SelectedValue.ToString(),
                    };


                    Task.Run(() => {

                        SetProcessStatus(true, "Process Audio Script ...");

                        var response = AudioProxy.Instance.AudioScript(request);
                        this.Invoke(new Action(() => {

                            txtAudioScript_ScriptText.Text = response.Data;

                            var listChunks = ChunkTextHelper.ChunkAudioScript(response.Data);

                            int count = 1;
                            foreach (var chunk in listChunks)
                            {
                                var chunkText = new ChunkTextItem();
                                chunkText.Id = count;

                                chunkText.ChunkText = chunk;
                                chunkText.Status = "Draft";


                                AddChunkTextItemToListView(chunkText, count, lvChunkScripts);
                                count++;

                                _ChunkTexts.Add(chunkText);
                            }


                        }));

                        SetProcessStatus(true, "Process SEO Metadata ...");


                        this.Invoke(new Action(() =>
                        {
                            var request_seo_metadata = new MainApiProxy.ViewModels.AudioScript_SEOMetadataRequestViewModel
                            {
                                Topic = txtAudioScript_Title.Text,
                                Script = txtAudioScript_Story.Text,

                            };

                            var response_seo_metadata = AudioProxy.Instance.SEOMetadata(request_seo_metadata);

                            txtAudioScript_SEOMetadata.Text = response_seo_metadata.Data;
                        }));
                        



                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }


        private void SetProcessStatus(bool isProcess, string text)
        {


            this.Invoke(new Action(() => {
                if (isProcess)
                {
                    lbProcessing.Visible = true;
                    lbProcessing.Text = text;
                    btnAudioScript_Execute.Enabled = false;

                }
                else
                {
                    lbProcessing.Visible = false;
                    btnAudioScript_Execute.Enabled = true;
                }
            }));

        }


        private void AddChunkTextItemToListView(ChunkTextItem chunkTextItem, int count, ListView lv)
        {
            this.Invoke(new Action(() => {
                ListViewItem itemAdd = new ListViewItem();
                itemAdd.Text = count.ToString();
                itemAdd.Tag = count.ToString();
                itemAdd.SubItems.Add(chunkTextItem.ChunkText);
                itemAdd.SubItems.Add(chunkTextItem.Status);
                lv.Items.Add(itemAdd);


                ListViewItemToChunkTextItem[itemAdd] = chunkTextItem;
                ChunkTextItemToListViewItem[chunkTextItem] = itemAdd;

            }));

        }

        private void lvChunkScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvChunkScripts.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvChunkScripts.SelectedItems[0].Tag);
                var item = _ChunkTexts.FirstOrDefault(x => x.Id == _Id);
                txtChunkScript_ChunkText.Text = item.ChunkText;
            }
        }

        private void btnAudioScript_ExecuteText2Speech_Click(object sender, EventArgs e)
        {


            if (chkAudioScript_Text2Speech_CheckAll.Checked) {

                var listBase64Audio = new List<string>();

                Task.Run(() => {
                    int count = 0;
                    foreach (var item in _ChunkTexts)
                    {

                        count++;
                        SetProcessStatus(true, $"Process Audio Generate {item.Id}/{_ChunkTexts.Count} ...");


                        this.Invoke(new Action(() =>
                        {
                            if (count == _ChunkTexts.Count && chkChunkScript_TextEndScript.Checked && !string.IsNullOrEmpty(txtChunkScript_TextEndScript.Text))
                            {
                                item.ChunkText = item.ChunkText + "\n\n" + txtChunkScript_TextEndScript.Text;
                            }

                        }));
                        


                        string responseAudio = GoogleGeminiHelper.GenerateText2Speech(chunkText: item.ChunkText);

                        dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseAudio);

                        try
                        {
                            string audioData = (string)dynamicObject.candidates[0].content.parts[0].inlineData.data;
                            string base64AudioString = audioData;                            
                            listBase64Audio.Add(base64AudioString);


                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show($"{item.Id}=> {ex.Message}");

                        }

                        this.Invoke(new Action(() =>
                        {
                            ((ListViewItem)ChunkTextItemToListViewItem[item]).SubItems[2].Text = "SUCCESS";

                        }));
                        

                        SetProcessStatus(false, "");
                        
                    }



                    this.Invoke(new Action(() =>
                    {
                        if (listBase64Audio.Count == _ChunkTexts.Count)
                        {
                            DateTime dateTime = DateTime.Now;
                            string dateFolderFormat = dateTime.ToString("yyyy_MM_dd");
                            string folderPath = $@"{Setting.Instance.Data.RootAudioOutputPath}\{StringHelper.ToSlug(txtAudioScript_Title.Text)}\{dateFolderFormat}";
                            if(!System.IO.Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);


                            StringBuilder stringBuilder = new StringBuilder();
                            string fileName = $"{StringHelper.UniqueKey(6)}.wav";

                            string filePath = Path.Combine($"{folderPath}", $"{fileName}");
                            AudioConverterHelper.ProcessAudioChunks(listBase64Audio, filePath);


                            int duration = MediaInfoHelper.GetDuration(filePath);

                            stringBuilder.AppendLine($"//Tạo ảnh ra video");
                            stringBuilder.AppendLine($"//Video dọc 9:16");
                            stringBuilder.AppendLine($"ffmpeg -r 1 -loop 1 -i 1.jpeg  -t {duration} -acodec copy -r 1 -vf scale=720:1280 -c:v libx264 -preset ultrafast -crf 21 static_video.mp4");
                            stringBuilder.AppendLine($"//Video ngang 16:9");
                            stringBuilder.AppendLine($"ffmpeg -r 1 -loop 1 -i 1.jpeg  -t {duration} -acodec copy -r 1 -vf scale=1920:1080 -c:v libx264 -preset ultrafast -crf 21 static_video.mp4");

                            stringBuilder.AppendLine($"//Chuyển wav audio sang mp3");
                            stringBuilder.AppendLine($"ffmpeg -i {fileName} output.mp3");

                            stringBuilder.AppendLine($"//Ghép video vào audio");
                            stringBuilder.AppendLine($"ffmpeg -i static_video.mp4 -i output.mp3 -c:v copy -c:a copy -shortest output.mp4");

                            txtAudioScript_FFMPEGCommandText.Text = stringBuilder.ToString();



                            txtAudioScript_FFMPEGCommandText.AppendText("🎵 Converting wav to mp3...");
                            AudioHelper.WaveToMp3(filePath, $@"{folderPath}\output.mp3");
                            txtAudioScript_FFMPEGCommandText.AppendText("🎵 Converted wav to mp3...");
                            txtAudioScript_FFMPEGCommandText.AppendText("🎵 Mixing background music...");
                            string backgroundMusic = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Music\\AudioBackgrounds\\SeamlesslyLoved.mp3";
                            AudioHelper.MixAudioWithMusic($@"{folderPath}\output.mp3", backgroundMusic, $@"{folderPath}\mixoutput.mp3");
                            txtAudioScript_FFMPEGCommandText.AppendText("🎵 Mixed background music...");


                        }
                        else
                        {
                            MessageBox.Show("Error not full chunks for merge audio");
                        }

                    }));

                   

                    
                });
               

            }
            else
            {
                if (lvChunkScripts.SelectedItems.Count > 0)
                {
                    int _Id = Convert.ToInt32(lvChunkScripts.SelectedItems[0].Tag);

                    Task.Run(() => {
                        SetProcessStatus(true, "Process Audio Generate ...");

                        var item = _ChunkTexts.FirstOrDefault(x => x.Id == _Id);
                        string responseAudio = GoogleGeminiHelper.GenerateText2Speech(chunkText: item.ChunkText);
                        dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseAudio);
                        try
                        {

                            this.Invoke(new Action(() =>
                            {
                                string audioData = (string)dynamicObject.candidates[0].content.parts[0].inlineData.data;
                                string base64AudioString = audioData;
                                DateTime dateTime = DateTime.Now;

                                string dateFolderFormat = dateTime.ToString("yyyy_MM_dd");
                                string folderPath = $@"{Setting.Instance.Data.RootAudioOutputPath}\{StringHelper.ToSlug(txtAudioScript_Title.Text)}\{dateFolderFormat}";
                                if (!System.IO.Directory.Exists(folderPath))
                                    Directory.CreateDirectory(folderPath);

                                string filePath = Path.Combine($"{folderPath}", $"{StringHelper.UniqueKey(6)}.wav");


                                AudioConverterHelper.ProcessAudioChunks(new List<string> { base64AudioString }, filePath);

                            }));
                          
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);

                        }

                        SetProcessStatus(false, "");
                    });


                }
            }


            

        }
    }



    public class ChunkTextItem
    {
        public int Id { get; set; }
        public string ChunkText { get; set; }
        public string Status { get; set; } = "";
    }
}
