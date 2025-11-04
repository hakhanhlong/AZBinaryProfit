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
                        TargetAudience = GenerateTitle_SelectTargetAudience.ValueMember.ToString(),
                        ToneStyle = GenerateTitle_SelectToneStyle.ValueMember.ToString(),
                        UseCase = GenerateTitle_SelectUseCase.ValueMember.ToString(),
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
    }
}
