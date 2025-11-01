using AZStoryVideoProfit.MainApiProxy;
using AZStoryVideoProfit.MainApiProxy.ViewModels;
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
    public partial class StoryForm : Form
    {
        public StoryForm()
        {
            InitializeComponent();
        }

        private void InitData()
        {
            selectPersonas.DataSource = StorySetting.Instance.Data.Personas;
            selectPersonas.DisplayMember = "Name";
            selectPersonas.ValueMember = "Description";


            selectLanguage.DataSource = new List<object>
            {
                new {Name = "English", Description="English"},
                new {Name = "Vietnamese", Description="Vietnamese"},
            };
            selectLanguage.DisplayMember = "Name";
            selectLanguage.ValueMember = "Description";
        }

        private void StoryForm_Load(object sender, EventArgs e)
        {
            InitData();
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                var request = new StoryIdeaRequestViewModel
                {
                    IdeaNumber = (int)txtNumberOfIdea.Value,
                    Language = (string)selectLanguage.SelectedValue,
                    Topic = txtIdeaText.Text,
                };
                SetProcessStatus(true);
                var jsonResult = StoryProxy.Instance.StoryIdea(request);
                txtResultIdea.Text = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
            }           
            finally
            {
                SetProcessStatus(false);
            }
           
            

        }

        private void SetProcessStatus(bool isProcess)
        {
            if (isProcess)
            {
                lbProcessing.Visible = true;
                btnExecute.Enabled = false;
            }
            else
            {
                lbProcessing.Visible = false;
                btnExecute.Enabled = true;
            }
        }
    }
}
