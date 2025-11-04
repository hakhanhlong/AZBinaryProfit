using AZStoryVideoProfit.MainApiProxy;
using AZStoryVideoProfit.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AZStoryVideoProfit.Forms
{
    public partial class YoutubeForm : Form
    {
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
    }
}
