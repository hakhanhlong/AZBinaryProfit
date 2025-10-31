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
    public partial class IdeaStoryForm : Form
    {
        public IdeaStoryForm()
        {
            InitializeComponent();
        }

        private void IdeaStoryForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
