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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axEDOffice1.OpenFileDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axEDOffice1.ExitOfficeApp();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (axEDOffice1.GetCurrentProgID() == "Word.Application")
            //{
                axEDOffice1.ProtectDoc(2);
            //}                
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axEDOffice1.PrintPreview();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void axViewer1_OnDocumentLoaded(object sender, EventArgs e)
        {

        }
    }
}
