using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace usercontrol_winform
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void Open()
        {
            try
            {
                axEDOffice1.OpenFileDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Protect()
        {
            if
            (axEDOffice1.GetCurrentProgID() == "Word.Application")
            {

                axEDOffice1.ProtectDoc(2);
            }
        }

        public void Print()
        {
            axEDOffice1.PrintPreview();

        }

        public void Close()
        {
            axEDOffice1.ExitOfficeApp();

        }
    }
}
