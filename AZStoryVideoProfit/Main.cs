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
        IdeaStoryForm _IdeaStoryForm;
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
                    _IdeaStoryForm = new IdeaStoryForm();
                    _IdeaStoryForm.MdiParent = this;
                    _IdeaStoryForm.Show();
                }
                else
                {
                    if (_IdeaStoryForm.IsDisposed)
                    {
                        _IdeaStoryForm = new IdeaStoryForm();
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
    }
}
