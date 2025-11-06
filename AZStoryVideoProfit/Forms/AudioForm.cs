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

                    foreach (var item in _ChunkTexts)
                    {


                        SetProcessStatus(true, $"Process Audio Generate {item.Id}/{_ChunkTexts.Count} ...");

                        string responseAudio = GoogleGeminiHelper.GenerateText2Speech(chunkText: item.ChunkText, apiKey: "AIzaSyA3bUX30qoxazBwYOMQ7rg2zO68iFy_3dw");

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
                            string dateFolderFormat = dateTime.ToString("YYYY_MM_dd");

                            string test = StringHelper.ToSlug(txtAudioScript_Title.Text);

                            string filePath = Path.Combine(Setting.Instance.Data.RootAudioOutputPath, $"{dateFolderFormat}", $"{StringHelper.UniqueKey(6)}.wav");

                            AudioConverterHelper.ProcessAudioChunks(listBase64Audio, filePath);
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
                        string responseAudio = GoogleGeminiHelper.GenerateText2Speech(chunkText: item.ChunkText, apiKey: "AIzaSyBvLmvOz_OUcWI2fVqwW56cCTy6ARQ-uNE");

                        dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseAudio);

                        try
                        {
                            string audioData = (string)dynamicObject.candidates[0].content.parts[0].inlineData.data;
                            string base64AudioString = audioData;
                            DateTime dateTime = DateTime.Now;

                            string dateFolderFormat = dateTime.ToString("YYYY_MM_dd");
                            string filePath = Path.Combine(Setting.Instance.Data.RootAudioOutputPath,
                                $"{StringHelper.ToSlug(txtAudioScript_Title.Text)}", $"{dateFolderFormat}", $"{StringHelper.UniqueKey(6)}.wav");

                            AudioConverterHelper.ProcessAudioChunks(new List<string> { base64AudioString }, filePath);
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
