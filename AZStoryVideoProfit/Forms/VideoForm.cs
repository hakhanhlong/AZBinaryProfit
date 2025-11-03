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
    public partial class VideoForm : Form
    {

        private List<StoryVideoSceneItem> _VideScenes;

        public List<StoryVideoSceneCharacter> _VideScenesCharacters;

        public VideoForm()
        {
            InitializeComponent();
        }


        private void InitData()
        {
            selectStoryVideoStyle.DataSource = StoryVideoSetting.Instance.Data.StoryVideoStyles;
            selectStoryVideoStyle.DisplayMember = "Name";
            selectStoryVideoStyle.ValueMember = "Description";


            selectVideoAspectRatio.DataSource = StoryVideoSetting.Instance.Data.StoryVideoAspectRatios;
            selectVideoAspectRatio.DisplayMember = "Name";
            selectVideoAspectRatio.ValueMember = "Description";
        }


        private void VideoForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            _VideScenes = new List<StoryVideoSceneItem>();
            _VideScenesCharacters = new List<StoryVideoSceneCharacter>();
            InitData();
        }


        private void AddVideoSceneItemToListView(StoryVideoSceneItem videoSceneItem, int count, ListView lv)
        {
            this.Invoke(new Action(() => {
                ListViewItem itemAdd = new ListViewItem();
                itemAdd.Text = count.ToString();
                itemAdd.Tag = count.ToString();
                itemAdd.SubItems.Add(videoSceneItem.Description);
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
                    
                }
                else
                {
                    lbProcessing.Visible = false;
                    btnExecute.Enabled = true;                    
                }
            }));

        }


        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new Action(() => {
                    var request = new StoryVideoSceneRequestViewModel
                    {
                        Genre = txtGenres.Text,
                        Num_Scenes = (int)txtNumberOfScene.Value,
                        Scene_Per_Second = (int)txtScenePerSecond.Value,
                        Story = txtStory.Text,
                        VideoStyle = (string)selectStoryVideoStyle.SelectedValue,
                        Video_AspectRatio = (string)selectVideoAspectRatio.SelectedValue
                    };


                    Task.Run(() => {
                        SetProcessStatus(true, "Process Story Scene ...");
                        var jsonResult = StoryVideoProxy.Instance.StoryVideoSceneGenerate(request);

                        if (jsonResult.Data != null && jsonResult.Data.Scenes.Any())
                        {
                            int count = 0;
                            foreach (var item in jsonResult.Data.Scenes)
                            {
                                count++;
                                AddVideoSceneItemToListView(item, count, lvScenes);
                                item.Id = count;
                                _VideScenes.Add(item);
                            }
                           
                        }


                        if (!_VideScenesCharacters.Any())
                        {
                            if (jsonResult.Data != null && jsonResult.Data.Characters.Any())
                            {
                                _VideScenesCharacters.AddRange(jsonResult.Data.Characters);
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

        private void lvScenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvScenes.SelectedItems.Count > 0)
            {
                int _Id = Convert.ToInt32(lvScenes.SelectedItems[0].Tag);
                var item = _VideScenes.FirstOrDefault(x => x.Id == _Id);
                txtViewScene.Text = JsonConvert.SerializeObject(new { Scene = item, Characters = _VideScenesCharacters }, Formatting.Indented);
            }
       }
    }
}
