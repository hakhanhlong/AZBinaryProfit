using AZStoryVideoProfit.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AZStoryVideoProfit
{
    public partial class Main : Form
    {
        StoryForm _IdeaStoryForm;
        VideoForm _VideoForm;

        YoutubeForm _YoutubeForm;
        public Main()
        {
            InitializeComponent();
        }

        private void IdeaStoryVideo_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {

                if (_IdeaStoryForm == null)
                {
                    _IdeaStoryForm = new StoryForm();
                    _IdeaStoryForm.MdiParent = this;
                    _IdeaStoryForm.Show();
                }
                else
                {
                    if (_IdeaStoryForm.IsDisposed)
                    {
                        _IdeaStoryForm = new StoryForm();
                        _IdeaStoryForm.MdiParent = this;
                        _IdeaStoryForm.Show();
                    }
                    else
                    {
                        _IdeaStoryForm.Activate();
                    }
                }
            }));


        }

        private void StoryVideo_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {

                if (_VideoForm == null)
                {
                    _VideoForm = new VideoForm();
                    _VideoForm.MdiParent = this;
                    _VideoForm.Show();
                }
                else
                {
                    if (_VideoForm.IsDisposed)
                    {
                        _VideoForm = new VideoForm();
                        _VideoForm.MdiParent = this;
                        _VideoForm.Show();
                    }
                    else
                    {
                        _VideoForm.Activate();
                    }
                }
            }));
        }

        private void btnYoutube_Click(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {

                if (_YoutubeForm == null)
                {
                    _YoutubeForm = new YoutubeForm();
                    _YoutubeForm.MdiParent = this;
                    _YoutubeForm.Show();
                }
                else
                {
                    if (_YoutubeForm.IsDisposed)
                    {
                        _YoutubeForm = new YoutubeForm();
                        _YoutubeForm.MdiParent = this;
                        _YoutubeForm.Show();
                    }
                    else
                    {
                        _YoutubeForm.Activate();
                    }
                }
            }));
        }
    }
}
