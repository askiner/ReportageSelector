using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using UnidecodeSharpFork;

namespace ReportageSelector
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public List<IReportage> storage = null;

        private void Main_Load(object sender, EventArgs e)
        {
            IReportageRepository rep = new ReportageRepository();

            storage = rep.GetReportageList();

            ReportagesListBox.DataSource = storage;

            ReportagesListBox.DisplayMember = "DisplayName";
            //ReportagesListBox.ValueMember = "Reference";

            NewReportageBox.Checked = true;
        }

        private void ReportagesListBoxSelected()
        {
            if (ReportagesListBox.SelectedValue != null)
            {
                NoReportageBox.Checked = false;
                NewReportageBox.Checked = false;
                VisibilityCheckBox.Enabled = false;
                PreviewBox.Load(((IReportage)ReportagesListBox.SelectedValue).ImageUrl);
            }
        }

        private void NewReportageBoxSelected()
        {
            if (NewReportageBox.Checked)
            {
                NoReportageBox.Checked = false;
                ReportagesListBox.SelectedItem = null;
                PreviewBox.Image = null;
                VisibilityCheckBox.Enabled = true;
                VisibilityCheckBox.Checked = true;
            }
        }

        private void NoReportageBoxSelected()
        {
            if (NoReportageBox.Checked)
            {
                NewReportageBox.Checked = false;
                ReportagesListBox.SelectedItem = null;
                PreviewBox.Image = null;
                VisibilityCheckBox.Enabled = false;
            }
        }

        private void ReportagesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportagesListBoxSelected();
        }

        private void NewReportageBox_CheckedChanged(object sender, EventArgs e)
        {
            NewReportageBoxSelected();
        }

        private void NoReportageBox_CheckedChanged(object sender, EventArgs e)
        {
            NoReportageBoxSelected();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {

            if (Program.Files != null && Program.Files.Count > 0)
            {
                string prefix = "";
                IProductionMethod methodToProduce;

                if (NoReportageBox.Checked)
                {
                    methodToProduce = new NoReportageMethod();
                }
                else
                    if (NewReportageBox.Checked)
                    {
                        string firstFileInList = Program.Files[0];

                        //if (File.Exists(firstFileInList))
                        //{
                        //    File
                        //}

                        if (VisibilityCheckBox.Checked)
                            methodToProduce = new VisibleReportageMethod();
                        else
                            methodToProduce = new HiddenReportageMethod();
                    }
                    else
                        if (ReportagesListBox.SelectedValue != null)
                        {
                            if ((ReportagesListBox.SelectedValue as IReportage) != null)
                            {
                                prefix = ((IReportage)ReportagesListBox.SelectedValue).Reference + "_";
                                methodToProduce = new VisibleReportageMethod();
                            }

                        
                        
                        }            

                if (Program.Files != null && Program.Files.Count > 0)
                {
                    foreach (string file in Program.Files)
                    {
                    
                    }
                }


            }       
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }
    }
}
