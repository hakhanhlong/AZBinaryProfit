using AZStoryVideoProfit.Helpers;
using AZStoryVideoProfit.MainApiProxy;
using AZStoryVideoProfit.Settings;
using AZStoryVideoProfit.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AZStoryVideoProfit.Forms
{
    public partial class YoutubeForm : Form
    {

        List<SceneLineTextItem> _SceneLineTextItems = new List<SceneLineTextItem>();
        List<string> _SceneImagePaths = new List<string>();
        YoutubeShortStoryVideoViewModel _YoutubeShortStoryVideoScripts = new YoutubeShortStoryVideoViewModel();




        List<SceneLineTextItem> _StoryVideoSceneLineTextItems = new List<SceneLineTextItem>();
        List<string> _StoryVideoSceneImagePaths = new List<string>();
        YoutubeStoryVideoViewModel _YoutubeStoryVideoScripts = new YoutubeStoryVideoViewModel();

        public YoutubeForm()
        {
            InitializeComponent();
        }

        private void InitData()
        {
            GenerateTitle_SelectTargetAudience.DataSource = YoutubeSetting.Instance.Data.GenerateTitle.TargetAudiences;
            GenerateTitle_SelectTargetAudience.DisplayMember = "Name";
            GenerateTitle_SelectTargetAudience.ValueMember = "Description";

            GenerateTitle_SelectToneStyle.DataSource = YoutubeSetting.Instance.Data.GenerateTitle.ToneStyles;
            GenerateTitle_SelectToneStyle.DisplayMember = "Name";
            GenerateTitle_SelectToneStyle.ValueMember = "Description";

            GenerateTitle_SelectUseCase.DataSource = YoutubeSetting.Instance.Data.GenerateTitle.UseCases;
            GenerateTitle_SelectUseCase.DisplayMember = "Name";
            GenerateTitle_SelectUseCase.ValueMember = "Description";




            GenerateDescription_SelectTargetAudience.DataSource = YoutubeSetting.Instance.Data.GenerateDescription.TargetAudiences;
            GenerateDescription_SelectTargetAudience.DisplayMember = "Name";
            GenerateDescription_SelectTargetAudience.ValueMember = "Description";

            GenerateDescription_SelectToneStyle.DataSource = YoutubeSetting.Instance.Data.GenerateDescription.ToneStyles;
            GenerateDescription_SelectToneStyle.DisplayMember = "Name";
            GenerateDescription_SelectToneStyle.ValueMember = "Description";

            GenerateDescription_SelectUseCase.DataSource = YoutubeSetting.Instance.Data.GenerateDescription.UseCases;
            GenerateDescription_SelectUseCase.DisplayMember = "Name";
            GenerateDescription_SelectUseCase.ValueMember = "Description";

            GenerateDescription_SelectSeoGoal.DataSource = YoutubeSetting.Instance.Data.GenerateDescription.SeoGoals;
            GenerateDescription_SelectSeoGoal.DisplayMember = "Name";
            GenerateDescription_SelectSeoGoal.ValueMember = "Description";




            ThumbnailConcept_TargetAudience.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.TargetAudiences;
            ThumbnailConcept_TargetAudience.DisplayMember = "Name";
            ThumbnailConcept_TargetAudience.ValueMember = "Description";

            ThumbnailConcept_ContentTypes.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.ContentTypes;
            ThumbnailConcept_ContentTypes.DisplayMember = "Name";
            ThumbnailConcept_ContentTypes.ValueMember = "Description";

            ThumbnailConcept_StylePreferences.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.StylePreferences;
            ThumbnailConcept_StylePreferences.DisplayMember = "Name";
            ThumbnailConcept_StylePreferences.ValueMember = "Description";







            ThumbnailImagePrompt_StylePreferences.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.StylePreferences;
            ThumbnailImagePrompt_StylePreferences.DisplayMember = "Name";
            ThumbnailImagePrompt_StylePreferences.ValueMember = "Description";

            ThumbnailImagePrompt_ImageTypes.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.ImageStyles;
            ThumbnailImagePrompt_ImageTypes.DisplayMember = "Name";
            ThumbnailImagePrompt_ImageTypes.ValueMember = "Description";

            ThumbnailImagePrompt_ImageFocus.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.ImageFocus;
            ThumbnailImagePrompt_ImageFocus.DisplayMember = "Name";
            ThumbnailImagePrompt_ImageFocus.ValueMember = "Description";

            ThumbnailImagePrompt_AspectRatio.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.AspectRatios;
            ThumbnailImagePrompt_AspectRatio.DisplayMember = "Name";
            ThumbnailImagePrompt_AspectRatio.ValueMember = "Description";

            ThumbnailImagePrompt_TextStyles.DataSource = YoutubeSetting.Instance.Data.GenerateThumbnail.TextStyles;
            ThumbnailImagePrompt_TextStyles.DisplayMember = "Name";
            ThumbnailImagePrompt_TextStyles.ValueMember = "Description";


            GenerateShortVideoScript_HookTypes.DataSource = YoutubeSetting.Instance.Data.GenerateShortScript.HookTypes;
            GenerateShortVideoScript_HookTypes.DisplayMember = "Name";
            GenerateShortVideoScript_HookTypes.ValueMember = "Description";

            GenerateShortVideoScript_HookInstructions.DataSource = YoutubeSetting.Instance.Data.GenerateShortScript.HookInstructions;
            GenerateShortVideoScript_HookInstructions.DisplayMember = "Name";
            GenerateShortVideoScript_HookInstructions.ValueMember = "Description";

            GenerateShortVideoScript_ContentTypes.DataSource = YoutubeSetting.Instance.Data.GenerateShortScript.ContentTypes;
            GenerateShortVideoScript_ContentTypes.DisplayMember = "Name";
            GenerateShortVideoScript_ContentTypes.ValueMember = "Description";

            GenerateShortVideoScript_ToneStyles.DataSource = YoutubeSetting.Instance.Data.GenerateShortScript.ToneStyles;
            GenerateShortVideoScript_ToneStyles.DisplayMember = "Name";
            GenerateShortVideoScript_ToneStyles.ValueMember = "Description";



            

            GenerateStoryVideo_ToneStyles.DataSource = YoutubeSetting.Instance.Data.GenerateStoryVideo.ToneStyles;
            GenerateStoryVideo_ToneStyles.DisplayMember = "Name";
            GenerateStoryVideo_ToneStyles.ValueMember = "Description";

            GenerateStoryVideo_Hooks.DataSource = YoutubeSetting.Instance.Data.GenerateStoryVideo.Hooks;
            GenerateStoryVideo_Hooks.DisplayMember = "Name";
            GenerateStoryVideo_Hooks.ValueMember = "Description";

            GenerateStoryVideo_Usecases.DataSource = YoutubeSetting.Instance.Data.GenerateStoryVideo.UseCases;
            GenerateStoryVideo_Usecases.DisplayMember = "Name";
            GenerateStoryVideo_Usecases.ValueMember = "Description";

            GenerateStoryVideo_ScriptStructures.DataSource = YoutubeSetting.Instance.Data.GenerateStoryVideo.ScriptStructures;
            GenerateStoryVideo_ScriptStructures.DisplayMember = "Name";
            GenerateStoryVideo_ScriptStructures.ValueMember = "Description";

            GenerateStoryVideo_CommunityInteractions.DataSource = YoutubeSetting.Instance.Data.GenerateStoryVideo.CommunityInteractions;
            GenerateStoryVideo_CommunityInteractions.DisplayMember = "Name";
            GenerateStoryVideo_CommunityInteractions.ValueMember = "Description";

            GenerateStoryVideo_Language.DataSource = new List<object>
            {
                new {Name = "English", Description="English"},
                new {Name = "Vietnamese", Description="Vietnamese"},
            };
            GenerateStoryVideo_Language.DisplayMember = "Name";
            GenerateStoryVideo_Language.ValueMember = "Description";





            GenerateStoryVideo_Narration_ToneStyles.DataSource = YoutubeSetting.Instance.Data.GenerateShortScript.ToneStyles;
            GenerateStoryVideo_Narration_ToneStyles.DisplayMember = "Name";
            GenerateStoryVideo_Narration_ToneStyles.ValueMember = "Description";


        }


        private void YoutubeForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            InitData();
        }

        private void SetProcessStatus(bool isProcess, string text)
        {


            this.Invoke(new Action(() => {
                if (isProcess)
                {
                    lbProcessing.Visible = true;
                    lbProcessing.Text = text;
                    btnGenerateTitle_Execute.Enabled = false;

                }
                else
                {
                    lbProcessing.Visible = false;
                    btnGenerateTitle_Execute.Enabled = true;
                }
            }));

        }

        private void btnGenerateTitle_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {

                    var request = new MainApiProxy.ViewModels.YoutubeGenerateTitleRequestViewModel
                    {
                        MainPoint = txtGenerateTitle_MainPoint.Text,
                        NumTitles = (int)GenerateTitle_TxtNumTitle.Value,
                        TargetAudience = GenerateTitle_SelectTargetAudience.SelectedValue.ToString(),
                        ToneStyle = GenerateTitle_SelectToneStyle.SelectedValue.ToString(),
                        UseCase = GenerateTitle_SelectUseCase.SelectedValue.ToString(),
                    };


                    Task.Run(() => {


                       

                        SetProcessStatus(true, "Process Generate Title ...");

                        var response = YoutubeProxy.Instance.GenerateTitle(request);
                        this.Invoke(new Action(() => {

                            txtGenerateTitle_Results.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                        }));


                        SetProcessStatus(false, "");
                    });

                }));

              


            }
            catch
            { SetProcessStatus(false, "");  }

        }

        private void btnGenerateDescription_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {

                    var request = new MainApiProxy.ViewModels.YoutubeGenerateDescriptionRequestViewModel
                    {
                        MainPoint = txtGenerateDescription_MainPoint.Text,
                        Seo_Goals = GenerateDescription_SelectSeoGoal.SelectedValue.ToString(),
                        TargetAudience = GenerateDescription_SelectTargetAudience.SelectedValue.ToString(),
                        ToneStyle = GenerateDescription_SelectToneStyle.SelectedValue.ToString(),
                        UseCase = GenerateDescription_SelectUseCase.SelectedValue.ToString(),
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Description ...");

                        var response = YoutubeProxy.Instance.GenerateDescription(request);
                        this.Invoke(new Action(() => {

                            txtGenerateDescription_Results.Text = response.Data;

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnThumbnailConcept_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {



                    var request = new MainApiProxy.ViewModels.YoutubeGenerateThumbnail_ThumbnailConceptRequestViewModel
                                {
                                    NumConcepts = (int)ThumbnailConcept_NumConcepts.Value,
                                    VideoTitle = txtThumbnailConcept_VideoTitle.Text,
                                    TargetAudience = ThumbnailConcept_TargetAudience.SelectedValue.ToString(),
                                    VideoDescription = txtThumbnailConcept_VideoDescription.Text,
                                    ContentType = ThumbnailConcept_ContentTypes.SelectedValue.ToString(),
                                    Style_Preference = ThumbnailConcept_StylePreferences.SelectedValue.ToString()
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Thumbnail Concept ...");

                        var response = YoutubeProxy.Instance.GenerateThumbnailConcept(request);
                        this.Invoke(new Action(() => {

                            txtThumbnailConcept_Results.Text = response.Data;

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnThumbnailImagePrompt_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {



                    var request = new MainApiProxy.ViewModels.YoutubeGenerateThumbnail_ThumbnailImagePromptRequestViewModel
                    {
                        Image_Focus = ThumbnailImagePrompt_ImageFocus.SelectedValue.ToString(),
                        Image_Style  = ThumbnailImagePrompt_ImageTypes.SelectedValue.ToString(),
                        Aspect_Ratio = ThumbnailImagePrompt_AspectRatio.SelectedValue.ToString(),
                        Concept_Description = txtThumbnailImagePrompt_ConceptDescription.Text,                        
                        Style_Preference = ThumbnailImagePrompt_StylePreferences.SelectedValue.ToString(),
                        Text_Style = ThumbnailImagePrompt_TextStyles.SelectedValue.ToString()
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Thumbnail Image Prompt ...");

                        var response = YoutubeProxy.Instance.GenerateThumbnail_ImagePrompt(request);
                        this.Invoke(new Action(() => {

                            txtThumbnailImagePrompt_Result.Text = response.Data;

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnGenerateIdea_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {



                    var request = new MainApiProxy.ViewModels.YoutubeIdeaRequestViewModel
                    {
                       Idea_Number = (int)GenerateIdea_TxtNumIdea.Value,
                       Topic = GenerateIdea_TxtTopic.Text
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Idea ...");

                        var response = YoutubeProxy.Instance.YoutubeIdea(request);
                        this.Invoke(new Action(() => {

                            txtGenerateIdea_Results.Text = JsonConvert.SerializeObject(response, Formatting.Indented);

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnGenerateShortVideoScript_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {



                    var request = new MainApiProxy.ViewModels.YoutubeShortVideoScriptRequest
                    {
                        story = GenerateShortVideoScript_TxtStory.Text,
                        duration_seconds = (int)GenerateShortVideoScript_DurationPerSeconds.Value,
                        content_type = GenerateShortVideoScript_ContentTypes.SelectedValue.ToString(),
                        hook_instructions = GenerateShortVideoScript_HookInstructions.SelectedValue.ToString(),
                        hook_type = GenerateShortVideoScript_HookTypes.SelectedValue.ToString(),
                        include_captions = GenerateShortVideoScript_IncludeCaption.Checked,
                        include_sound_effects = GenerateShortVideoScript_IncludeSoundEffect.Checked,
                        include_text_overlay = GenerateShortVideoScript_IncludeTextOverlay.Checked,
                        vertical_framing_notes = GenerateShortVideoScript_VerticalFramingNote.Checked
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Short Video Script ...");

                        var response = YoutubeProxy.Instance.YoutubeShortVideoScript(request);
                        this.Invoke(new Action(() => {

                            GenerateShortVideoScript_TxtResult.Text = response.Data;

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnGenerateShortVideoScript_CreateNarration_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {


                    _YoutubeShortStoryVideoScripts = JsonConvert.DeserializeObject<YoutubeShortStoryVideoViewModel>(GenerateShortVideoScript_TxtResult.Text.Replace("```json", "").Replace("```", ""));

                    var request = new MainApiProxy.ViewModels.YoutubeShortVideoScriptNarrationRequest
                    {
                        duration_seconds = (int)GenerateShortVideoScript_DurationPerSeconds.Value,
                        shorts_script = GenerateShortVideoScript_TxtResult.Text,
                        tone_style = GenerateShortVideoScript_ToneStyles.SelectedValue.ToString()
                    };


                    Task.Run(() => {


                        SetProcessStatus(true, "Process Generate Short Video Script Narration...");

                        var response = YoutubeProxy.Instance.YoutubeShortVideoScriptNarration(request);
                        this.Invoke(new Action(() =>
                        {
                            GenerateShortVideoScript_TxtScriptNarration.Text = response.Data;

                        }));



                        //1.Sections

                        int totalScenes = _YoutubeShortStoryVideoScripts.Sections.SelectMany(x => x.Scenes).SelectMany(x=> x.TextOverlays).Count();
                        int sceneNumber = 1;
                        foreach (var section in _YoutubeShortStoryVideoScripts.Sections)
                        {

                            //2.Scenes
                            
                            foreach (var scene in section.Scenes)
                            {
                                //3.TextOverlay
                                foreach (var textOverlay in scene.TextOverlays)
                                {

                                    this.Invoke(new Action(() =>
                                    {
                                        var visualDesc = scene.VisualInstructions.FirstOrDefault(x => x.Id == textOverlay.VisualId);
                                        var voiceOver = scene.AudioVoiceover.FirstOrDefault(x => x.VisualId == textOverlay.VisualId);

                                        string prompt = $"Create a vertical (9:16) image for YouTube Shorts video.\n" +
                                         $"Scene {sceneNumber} of {totalScenes}:\n" +
                                         $"Visual Description: {visualDesc.Description}\n" +
                                         $"Context: {voiceOver.Description}\n" +

                                         "Style Requirements:\n" +
                                         "- High contrast and vibrant colors for better mobile viewing\n" +
                                         "- Clear focal point in the center for vertical format\n" +
                                         "- Professional quality, cinematic lighting\n" +
                                         "- Text-safe areas on top and bottom\n" +
                                         "- Visually distinct from other scenes\n" +
                                         "- Modern, engaging composition\n" +
                                         $"- Text Overlays \"{textOverlay.Description}\" \n" +
                                         $"- Suitable for {GenerateShortVideoScript_ContentTypes.SelectedValue} style content\n" +
                                         $"- Character must consistent\n" +

                                         "Technical Requirements:\n" +
                                         "- Vertical 9:16 aspect ratio\n" +
                                         "- High resolution, sharp details\n" +
                                         "- No text or watermarks\n" +
                                         "- No blurry or low-quality elements";


                                        var sceneLine = new SceneLineTextItem
                                        {
                                            Id = sceneNumber,
                                            SceneImagePrompt = prompt,
                                            Status = "Draft"
                                        };

                                        _SceneLineTextItems.Add(sceneLine);
                                        sceneNumber++;

                                    }));
                                 
                                }
                               
                            }
                        }
                     
                        foreach (var line in _SceneLineTextItems)
                        {                           
                            AddSceneLineTextItemToListView(line, line.Id, lvSceneLines);                                                       
                        }


                        

                        try
                        {

                            DateTime dateTime = DateTime.Now;
                            string dateFolderFormat = dateTime.ToString("yyyy_MM_dd");
                            string folderPath = $@"{Setting.Instance.Data.RootShortStoryVideoOutputPath}\{dateFolderFormat}\{StringHelper.UniqueKey(8)}";
                            if (!System.IO.Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);


                            //tạo audio Narration
                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText("1. Creating audio narration...\n");
                            }));
                            string scriptNarration = string.Empty;
                            this.Invoke(new Action(() => {
                                scriptNarration = GenerateShortVideoScript_TxtScriptNarration.Text;
                            }));


                            string responseAudio = GoogleGeminiHelper.GenerateText2Speech(chunkText: scriptNarration);
                            //dynamic dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseAudio);


                            string audioData = StringHelper.GetStringBetween(responseAudio, "\"data\": \"", "\"");//(string)dynamicObject.candidates[0].content.parts[0].inlineData.data;


                            string audioFileName = $"{StringHelper.UniqueKey(6)}.wav";
                            string audioFilePath = Path.Combine($"{folderPath}", $"{audioFileName}");
                            AudioConverterHelper.ProcessAudioChunks(new List<string> { audioData }, audioFilePath);
                            
                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText($"1.1 Created audio narration with name {audioFileName}...\n");
                            }));



                            AudioHelper.WaveToMp3(audioFilePath, $@"{folderPath}\audio_narration.mp3");
                            audioFilePath = $@"{folderPath}\audio_narration.mp3";

                            this.Invoke(new Action(() =>
                            {
                                GenerateShortVideoScript_TxtLogs.AppendText($"1.2 Converted audio .wav narration to .mp3...\n");
                                GenerateShortVideoScript_TxtLogs.AppendText($"1.3 Mix background music to narration audio...\n");
                            }));

                            string backgroundMusic = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\Music\\AudioBackgrounds\\TheLeavesThatFall.mp3";
                            AudioHelper.MixAudioWithMusic($@"{audioFilePath}", backgroundMusic, $@"{folderPath}\audio_narration_mixed.mp3");
                            audioFilePath = $@"{folderPath}\audio_narration_mixed.mp3";

                            this.Invoke(new Action(() =>
                            {
                                GenerateShortVideoScript_TxtLogs.AppendText("🎵 Mixed background music...");
                            }));

                            //tạo image
                            string promptText = string.Empty;
                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText("2. Creating scane image ...\n");
                            }));
                            
                            sceneNumber = 1;
                            foreach (var scene in _SceneLineTextItems)
                            {
                                Thread.Sleep(10000);

                                this.Invoke(new Action(() => {
                                    GenerateShortVideoScript_TxtLogs.AppendText($"- Creating scene  {sceneNumber} of {totalScenes}...\n");
                                }));


                                promptText = JsonConvert.SerializeObject(scene.SceneImagePrompt);
                                string responseImage = GoogleGeminiHelper.GenerateText2Image(promptText, aspectRatio: "9:16");
                                //dynamicObject = JsonConvert.DeserializeObject<dynamic>(responseImage);
                                //string imageData = (string)dynamicObject.candidates[0].content.parts[0].inlineData.data;

                                string imageData = StringHelper.GetStringBetween(responseImage, "\"data\": \"", "\"");
                                string base64ImageString = imageData;
                                if (string.IsNullOrEmpty(imageData))
                                {
                                    responseImage = GoogleGeminiHelper.GenerateText2Image(promptText, aspectRatio: "9:16");
                                    imageData = StringHelper.GetStringBetween(responseImage, "\"data\": \"", "\"");
                                    base64ImageString = imageData;
                                }

                                if (!System.IO.Directory.Exists($@"{folderPath}/Scenes"))
                                    Directory.CreateDirectory($@"{folderPath}/Scenes");
                                if(!string.IsNullOrEmpty(base64ImageString))
                                {
                                    string sceneImagePath = $@"{folderPath}/Scenes/{sceneNumber}_{totalScenes}.png";
                                    ImageConverterHelper.ConvertBase64ToPng(base64ImageString, sceneImagePath);
                                    _SceneImagePaths.Add(sceneImagePath);
                                }
                               

                                sceneNumber++;
                                this.Invoke(new Action(() => {
                                    GenerateShortVideoScript_TxtLogs.AppendText($"- Wait 10s for next scene...\n");
                                }));
                                
                                
                            }
                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText("Created scenes image ...\n");

                                GenerateShortVideoScript_TxtLogs.AppendText("3. Creating short video ...\n");
                            }));
                           

                            var videoCreator = new StoryVideoCreationHelper(folderPath);

                          

                            int audio_duration = MediaInfoHelper.GetDuration(audioFilePath);
                            int durationPerImage = (int)Math.Ceiling(Convert.ToDecimal(audio_duration/totalScenes));

                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText($"Frame per seconds: 12\n");
                                GenerateShortVideoScript_TxtLogs.AppendText($"Duration per image: {durationPerImage}\n");
                            }));

                            videoCreator.CreateVideo(_SceneImagePaths, audioFilePath, 12, durationPerImage);

                            this.Invoke(new Action(() => {
                                GenerateShortVideoScript_TxtLogs.AppendText("3. Created short video ...\n");
                            }));
                            
                            
                        }
                        catch (Exception ex){
                            MessageBox.Show(ex.Message);
                        }








                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }



        private void AddSceneLineTextItemToListView(SceneLineTextItem sceneLineTextItem, int count, ListView lv)
        {
            this.Invoke(new Action(() => {
                ListViewItem itemAdd = new ListViewItem();
                itemAdd.Text = count.ToString();
                itemAdd.Tag = count.ToString();
                itemAdd.SubItems.Add(sceneLineTextItem.SceneImagePrompt);
                itemAdd.SubItems.Add(sceneLineTextItem.Status);
                lv.Items.Add(itemAdd);                

            }));

        }


        private void AddStoryVideoSceneLineTextItemToListView(SceneLineTextItem sceneLineTextItem, int count, ListView lv)
        {
            this.Invoke(new Action(() => {
                ListViewItem itemAdd = new ListViewItem();
                itemAdd.Text = count.ToString();
                itemAdd.Tag = count.ToString();
                itemAdd.SubItems.Add(sceneLineTextItem.SceneImagePrompt);
                itemAdd.SubItems.Add(sceneLineTextItem.Status);
                lv.Items.Add(itemAdd);

            }));

        }


        public List<string> ExtractAudioLines(string script)
        {
            // Equivalent to: scenes = re.split(r'\n\n+', script)
            // Splits the script by one or more consecutive newline characters.
            //string[] scenes = Regex.Split(script, @"\n\n+");
            string[] scenes = Regex.Split(script, @"\n\n");

            List<string> audioLines = new List<string>();

            foreach (string scene in scenes)
            {

                if(scene.Contains("Visual Instructions:"))
                {
                    var visual_desc = scene.Split(new string[] { "Visual Instructions:" }, StringSplitOptions.None);
                    var test = visual_desc[1].Split('\n');
                }

                

                string pattern = @"\*+\s*Audio/Voiceover:\s*((\s|\S)*)";

                Match audioMatch = Regex.Match(scene, pattern, RegexOptions.IgnoreCase);

                if (audioMatch.Success)
                {


                    string patternAudio = @"^\s*\*\s*(.*)$";

                    // RegexOptions.Multiline is key here to treat '^' as the start of a line
                    // and not just the start of the entire string.
                    MatchCollection matches = Regex.Matches(audioMatch.Groups[1].Value.Trim(), patternAudio, RegexOptions.Multiline);

                    List<string> individualLines = new List<string>();

                    foreach (Match match in matches)
                    {
                        // Group[1] contains the text after the bullet and whitespace
                        audioLines.Add(match.Groups[1].Value.Trim());
                    }


                    
                }
            }

            return audioLines;
        }

        private void lvSceneLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSceneLines.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvSceneLines.SelectedItems[0].Tag);
                var item = _SceneLineTextItems.FirstOrDefault(x => x.Id == _Id);
                GenerateShortVideoScript_TxtSceneDetail.Text = item.SceneImagePrompt;
            }
       }

        private void btnGenerateStoryVideo_Execute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {



                    var request = new MainApiProxy.ViewModels.YoutubeGenerateStoryVideoScriptRequestViewModel
                    {
                        story = GenerateStoryVideo_TxtStory.Text,
                        duration_seconds = (int)GenerateStoryVideo_DurationPerSeconds.Value,
                        target_audience = GenerateStoryVideo_TxtTargetAudiences.Text,
                        tone_style = GenerateStoryVideo_ToneStyles.SelectedValue.ToString(),
                        use_case = GenerateStoryVideo_Usecases.SelectedValue.ToString(),
                        script_structure = ((YoutubeGenerateStoryVideo_BaseViewModel)GenerateStoryVideo_ScriptStructures.SelectedItem).Name,
                        script_structure_desc = GenerateStoryVideo_ScriptStructures.SelectedValue.ToString(),
                        hooks = GenerateStoryVideo_Hooks.SelectedValue.ToString(),
                        community_interactions = GenerateStoryVideo_CommunityInteractions.SelectedValue.ToString(),
                        include_hook = GenerateStoryVideo_IncludeHook.Checked,
                        include_cta = GenerateStoryVideo_IncludeCTA.Checked,
                        include_engagement = GenerateStoryVideo_IncludeEngagement.Checked,
                        include_timestamps = GenerateStoryVideo_IncludeTimestamp.Checked,
                        include_visual_cues = GenerateStoryVideo_IncludeVisualCue.Checked,
                        language = GenerateStoryVideo_Language.SelectedValue.ToString(),
                        
                    };


                    Task.Run(() => {




                        SetProcessStatus(true, "Process Generate Story Video Script ...");

                        var response = YoutubeProxy.Instance.GenerateStoryVideoScript(request);
                        this.Invoke(new Action(() => {

                            GenerateStoryVideo_TxtScriptResult.Text = response.Data;

                        }));


                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }
        }

        private void btnGenerateStoryVideo_CreateNarration_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((Action)(() => {


                    _YoutubeStoryVideoScripts = JsonConvert.DeserializeObject<YoutubeStoryVideoViewModel>(GenerateStoryVideo_TxtScriptResult.Text.Replace("```json", "").Replace("```", ""));

                    var request = new MainApiProxy.ViewModels.YoutubeStoryVideoScriptNarrationRequest
                    {
                        duration_seconds = (int)GenerateStoryVideo_DurationPerSeconds.Value,
                        story_video_script = GenerateStoryVideo_TxtScriptResult.Text,
                        tone_style = GenerateStoryVideo_Narration_ToneStyles.SelectedValue.ToString()
                    };


                    Task.Run(() => {


                        SetProcessStatus(true, "Process Generate Story Video Script Narration...");

                        var response = YoutubeProxy.Instance.YoutubeStoryVideoScriptNarration(request);
                        this.Invoke(new Action(() =>
                        {
                            GenerateStoryVideo_TxtNarrationText.Text = response.Data;

                        }));



                        //1.Sections

                        int totalScenes = _YoutubeStoryVideoScripts.Sections.SelectMany(x => x.Scenes).SelectMany(x => x.TextOverlays).Count();
                        int sceneNumber = 1;
                        foreach (var section in _YoutubeStoryVideoScripts.Sections)
                        {

                            //2.Scenes

                            foreach (var scene in section.Scenes)
                            {
                                //3.TextOverlay
                                foreach (var textOverlay in scene.TextOverlays)
                                {

                                    this.Invoke(new Action(() =>
                                    {
                                        var visualDesc = scene.VisualInstructions.FirstOrDefault(x => x.Id == textOverlay.VisualId);
                                        var voiceOver = scene.AudioVoiceover.FirstOrDefault(x => x.VisualId == textOverlay.VisualId);

                                        string prompt = $"Create a vertical (9:16) image for YouTube Shorts video.\n" +
                                         $"Scene {sceneNumber} of {totalScenes}:\n" +
                                         $"Visual Description: {visualDesc.Description}\n" +
                                         $"Context: {voiceOver.Description}\n" +

                                         "Style Requirements:\n" +
                                         "- High contrast and vibrant colors for better mobile viewing\n" +
                                         "- Clear focal point in the center for vertical format\n" +
                                         "- Professional quality, cinematic lighting\n" +
                                         "- Text-safe areas on top and bottom\n" +
                                         "- Visually distinct from other scenes\n" +
                                         "- Modern, engaging composition\n" +
                                         $"- Text Overlays \"{textOverlay.Description}\" \n" +
                                         //$"- Suitable for {GenerateShortVideoScript_ContentTypes.SelectedValue} style content\n" +
                                         $"- Character must consistent\n" +

                                         "Technical Requirements:\n" +
                                         "- Vertical 9:16 aspect ratio\n" +
                                         "- High resolution, sharp details\n" +
                                         "- No text or watermarks\n" +
                                         "- No blurry or low-quality elements";


                                        var sceneLine = new SceneLineTextItem
                                        {
                                            Id = sceneNumber,
                                            SceneImagePrompt = prompt,
                                            Status = "Draft"
                                        };

                                        _StoryVideoSceneLineTextItems.Add(sceneLine);
                                        sceneNumber++;

                                    }));

                                }

                            }
                        }

                        foreach (var line in _StoryVideoSceneLineTextItems)
                        {
                            AddStoryVideoSceneLineTextItemToListView(line, line.Id, lvStoryVideoSceneLine);
                        }




                        








                        SetProcessStatus(false, "");
                    });

                }));




            }
            catch
            { SetProcessStatus(false, ""); }


        }
    }

    public class AudioLineTextItem
    {
        public int Id { get; set; }
        public string AudioLine { get; set; }
        public string Status { get; set; } = "";
    }

    public class SceneLineTextItem
    {
        public int Id { get; set; }
        public string SceneImagePrompt { get; set; }
        public string Status { get; set; } = "";
    }


}
