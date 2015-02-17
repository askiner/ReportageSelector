using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReportageSelector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            PreviewBox.Load(@"http://tassphoto.com/thu_web/00000009224/10192756.thw");
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            PreviewBox.Load(@"http://tassphoto.com/thu_web/00000009226/10195206.thw");
        }
    }
}
