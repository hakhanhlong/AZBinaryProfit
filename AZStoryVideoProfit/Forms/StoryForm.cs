using AZStoryVideoProfit.MainApiProxy;
using AZStoryVideoProfit.MainApiProxy.ViewModels;
using AZStoryVideoProfit.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace AZStoryVideoProfit.Forms
{
    public partial class StoryForm : Form
    {

        private List<StoryIdeaItemViewModel> _StoryIdeas;
        private List<StoryIdeaItemViewModel> _StoryIdeaItemsSelected;

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
            _StoryIdeas = new List<StoryIdeaItemViewModel>();
            _StoryIdeaItemsSelected = new List<StoryIdeaItemViewModel>();
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {


            try
            {
                this.Invoke(new Action(() => {
                    var request = new StoryIdeaRequestViewModel
                    {
                        IdeaNumber = (int)txtNumberOfIdea.Value,
                        Language = (string)selectLanguage.SelectedValue,
                        Topic = txtIdeaText.Text,
                    };


                    Task.Run(() => {
                        SetProcessStatus(true);
                        var jsonResult = StoryProxy.Instance.StoryIdea(request);

                        if (jsonResult.Data != null && jsonResult.Data.IdeaStories.Any())
                        {
                            int count = 0;
                            foreach (var item in jsonResult.Data.IdeaStories)
                            {
                                count++;
                                AddIdeaItemToListView(item, count, lvResultIdeas);
                                item.Id = count;
                                _StoryIdeas.Add(item);
                            }
                        }
                        SetProcessStatus(false);
                    });
                    

                }));
               
               

            }
            catch
            {
                SetProcessStatus(false);
            }



           

                                  
        }

        private void AddIdeaItemToListView(StoryIdeaItemViewModel ideaItemViewModel, int count, ListView lv)
        {
            this.Invoke(new Action(() => {
                ListViewItem itemAdd = new ListViewItem();
                itemAdd.Text = count.ToString();
                itemAdd.Tag = count.ToString();
                itemAdd.SubItems.Add(ideaItemViewModel.Name);                      
                lv.Items.Add(itemAdd);                
            }));

        }

        private void SetProcessStatus(bool isProcess)
        {


            this.Invoke(new Action(() => {
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
            }));
            
        }

        private void lvResultIdeas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvResultIdeas.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvResultIdeas.SelectedItems[0].Tag);

                var item = _StoryIdeas.FirstOrDefault(x => x.Id == _Id);
                txtViewResultIdea.Text = JsonConvert.SerializeObject(item, Formatting.Indented);

                var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story" ,"StorySetting", "skprompt.txt");
                var personaContent = System.IO.File.ReadAllText(promptFilePath);
                personaContent = personaContent.Replace("{{$story}}", item.Story);
                personaContent = personaContent.Replace("{{$language}}", selectLanguage.SelectedValue.ToString());
                personaContent = personaContent.Replace("{{$persona}}", selectPersonas.SelectedValue.ToString());
                personaContent = personaContent.Replace("{{$story_setting}}", item.Story_Setting);
                personaContent = personaContent.Replace("{{$character_input}}", item.Character);
                personaContent = personaContent.Replace("{{$plot_element}}", item.Plot_Elements);
                personaContent = personaContent.Replace("{{$writing_style}}", item.Story_Writing_Style);
                personaContent = personaContent.Replace("{{$story_tone}}", item.Story_Tone);
                personaContent = personaContent.Replace("{{$narrative_pov}}", item.Narrative_Pov);
                personaContent = personaContent.Replace("{{$audience_age_group}}", item.Audience_Age_Group);
                personaContent = personaContent.Replace("{{$content_rating}}", item.Content_Rating);
                personaContent = personaContent.Replace("{{$ending_preference}}", item.Ending_Preference);

                txtViewStorySetting.Text = personaContent;



            }
        }
       
    }
}
