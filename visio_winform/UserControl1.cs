﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace visio_winform
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void Open()
        {
            axEDOffice1.OpenFileDialog();
        }

        public void Close()
        {
            axEDOffice1.ExitOfficeApp();
        }

        public void ReadOnly()
        {
            if (axEDOffice1.GetCurrentProgID() == "Word.Application")
            {
                axEDOffice1.ProtectDoc(2);
            }
        }

        public void Preview()
        {
            axEDOffice1.PrintPreview();
        }
    }
}
