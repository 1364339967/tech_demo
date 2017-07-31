using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace visio_winform
{
    public partial class Form_UseUserControl : Form
    {
        public Form_UseUserControl()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.userControl11.Open();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.userControl11.Close();
        }
    }
}
