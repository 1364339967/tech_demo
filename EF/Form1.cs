using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                EMMS_JCWEntities1 mEMMS_JCWEntities = new EMMS_JCWEntities1();
                PPT_Page mPPT_Page = new PPT_Page()
                {
                    Name = "这是EF测试"
                };
                mEMMS_JCWEntities.PPT_Page.Add(mPPT_Page);
                mEMMS_JCWEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
