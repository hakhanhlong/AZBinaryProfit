using AZStoryVideoProfit.Helpers;
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
                        SetProcessStatus(true, "Process Make Story Idea ...");
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
                        SetProcessStatus(false, "");
                    });
                    

                }));
               
               

            }
            catch (Exception ex)
            {
                SetProcessStatus(false, ex.Message);
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

        private void SetProcessStatus(bool isProcess, string text)
        {


            this.Invoke(new Action(() => {
                if (isProcess)
                {
                    lbProcessing.Visible = true;
                    lbProcessing.Text = text;
                    btnExecute.Enabled = false;
                    btnStoryPremise.Enabled = false;
                }
                else
                {
                    lbProcessing.Visible = false;
                    btnExecute.Enabled = true;
                    btnStoryPremise.Enabled = true;
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


                try
                {
                    this.Invoke(new Action(() => {

                        var request = new StorySettingRequestViewModel
                        {

                            Audience_Age_Group = item.Audience_Age_Group,
                            Character = item.Character,
                            Content_Rating = item.Content_Rating,
                            Ending_Preference = item.Ending_Preference,
                            Genre = item.Genre,
                            Guidelines = item.Guidelines,
                            Language = selectLanguage.SelectedValue.ToString(),
                            Name = item.Name,
                            Narrative_Pov = item.Narrative_Pov,
                            Persona = selectPersonas.SelectedValue.ToString(),
                            Plot_Elements = item.Plot_Elements,
                            Story = item.Story,
                            Story_Tone = item.Story_Tone,
                            Story_Setting = item.Story_Setting,
                            Story_Writing_Style = item.Story_Writing_Style,
                            NumberPages = (int)txtNumberOfPages.Value,
                            NumberWords = (int)txtNumberOfWords.Value
                            
                        };
                       
                        Task.Run(() => {
                            SetProcessStatus(true, "Process Story Setting ...");

                            var response = StoryProxy.Instance.StorySetting(request);
                            this.Invoke(new Action(() => {

                                txtViewStorySetting.Text = JsonConvert.SerializeObject(response, Formatting.Indented);
                                txtViewStorySetting.Text = txtViewStorySetting.Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");

                            }));


                            SetProcessStatus(false, "");
                        });

                    }));


                    }
                catch 
                { }
                             
            }
        }

        private void btnStoryPremise_Click(object sender, EventArgs e)
        {

            if (lvResultIdeas.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvResultIdeas.SelectedItems[0].Tag);
                var item = _StoryIdeas.FirstOrDefault(x => x.Id == _Id);


                try
                {
                    this.Invoke(new Action(() => {


                        var storySettingResponseViewModel = JsonConvert.DeserializeObject<StorySettingResponseViewModel>(txtViewStorySetting.Text);

                        var request = new StoryPremiseRequestViewModel
                        {

                            Persona = storySettingResponseViewModel.Settings,
                            Character = item.Character,
                            StorySetting = item.Story_Setting


                        };

                        Task.Run(() => {
                            SetProcessStatus(true, "Process Story Premise ...");

                            var response = StoryProxy.Instance.StoryPremise(request);
                            this.Invoke(new Action(() => {

                                txtViewStoryPremise.Text = response.Data;//JsonConvert.SerializeObject(response, Formatting.Indented);

                            }));


                            SetProcessStatus(false, "");
                        });

                    }));


                }
                catch
                { }

            }

            
            
        }

        private void btnStoryOutline_Click(object sender, EventArgs e)
        {
            if (lvResultIdeas.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvResultIdeas.SelectedItems[0].Tag);
                var item = _StoryIdeas.FirstOrDefault(x => x.Id == _Id);


                try
                {
                    this.Invoke(new Action(() => {


                        var storySettingResponseViewModel = JsonConvert.DeserializeObject<StorySettingResponseViewModel>(txtViewStorySetting.Text);
                        //var storyPremiseResponseViewMidel = JsonConvert.DeserializeObject<StoryPremiseResponseViewModel>(txtViewStoryPremise.Text);


                        


                        var request = new StoryOutlineRequestViewModel
                        {

                            Persona = storySettingResponseViewModel.Settings,
                            Premise = txtViewStoryPremise.Text//storyPremiseResponseViewMidel.Data


                        };

                        Task.Run(() => {
                            SetProcessStatus(true, "Process Story Outline ...");

                            var response = StoryProxy.Instance.StoryOutline(request);
                            this.Invoke(new Action(() => {

                                txtViewStoryOutline.Text = response.Data;//JsonConvert.SerializeObject(response, Formatting.Indented);

                            }));


                            SetProcessStatus(false, "");
                        });

                    }));


                }
                catch
                { }

            }
        }

        private void btnStoryStarting_Click(object sender, EventArgs e)
        {
            if (lvResultIdeas.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvResultIdeas.SelectedItems[0].Tag);
                var item = _StoryIdeas.FirstOrDefault(x => x.Id == _Id);


                try
                {
                    this.Invoke(new Action(() =>
                    {


                        var storySettingResponseViewModel = JsonConvert.DeserializeObject<StorySettingResponseViewModel>(txtViewStorySetting.Text);
                        //var storyPremiseResponseViewMidel = JsonConvert.DeserializeObject<StoryPremiseResponseViewModel>(txtViewStoryPremise.Text);
                       // var storyOutlineResponseViewMidel = JsonConvert.DeserializeObject<StoryOutlineResponseViewModel>(txtViewStoryOutline.Text);





                        var request = new StoryStartingRequestViewModel
                        {

                            Persona = storySettingResponseViewModel.Settings,
                            Premise = txtViewStoryPremise.Text,//storyPremiseResponseViewMidel.Data,
                            Outline = txtViewStoryOutline.Text,//storyOutlineResponseViewMidel.Data,
                            Guidelines = storySettingResponseViewModel.Guidelines


                        };

                        Task.Run(() =>
                        {
                            SetProcessStatus(true, "Process Story Starting ...");

                            var response = StoryProxy.Instance.StoryStarting(request);
                            this.Invoke(new Action(() =>
                            {

                                txtViewStoryStarting.Text = response.Data;
                                lbStoryStartingWordCount.Text = $"{StringHelper.CountWords(response.Data)} words";

                            }));


                            SetProcessStatus(false, "");
                        });

                    }));


                }
                catch
                { }
            }
        }

        private void btnStoryContinuationPlusEnd_Click(object sender, EventArgs e)
        {
            if (lvResultIdeas.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvResultIdeas.SelectedItems[0].Tag);
                var item = _StoryIdeas.FirstOrDefault(x => x.Id == _Id);


                try
                {
                    this.Invoke(new Action(() =>
                    {


                        var storySettingResponseViewModel = JsonConvert.DeserializeObject<StorySettingResponseViewModel>(txtViewStorySetting.Text);
                        //var storyPremiseResponseViewMidel = JsonConvert.DeserializeObject<StoryPremiseResponseViewModel>(txtViewStoryPremise.Text);
                        //var storyOutlineResponseViewMidel = JsonConvert.DeserializeObject<StoryOutlineResponseViewModel>(txtViewStoryOutline.Text);






                        var request = new StoryContinuationRequestViewModel
                        {

                            Persona = storySettingResponseViewModel.Settings,
                            Premise = txtViewStoryPremise.Text,//storyPremiseResponseViewMidel.Data,
                            Outline = txtViewStoryOutline.Text,//storyOutlineResponseViewMidel.Data,
                            Guidelines = storySettingResponseViewModel.Guidelines,
                            StoryStarting = txtViewStoryStarting.Text,
                            Number_Pages = (int)txtNumberOfPages.Value,
                            Number_Words = (int)txtNumberOfWords.Value


                        };

                        Task.Run(() =>
                        {
                            SetProcessStatus(true, "Process Story Continuation ...");

                            var response = StoryProxy.Instance.StoryContinuation(request);
                            this.Invoke(new Action(() =>
                            {

                                txtStoryContinuationPlusEnd.Text = response.Data;

                            }));


                            SetProcessStatus(false, "");
                        });

                    }));


                }
                catch
                { }
            }
        }
        
    }
}
