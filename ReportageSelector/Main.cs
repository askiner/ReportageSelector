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
        private BackgroundWorker bw = new BackgroundWorker();

        public Main()
        {
            InitializeComponent();
        }

        public List<IReportage> storage = null;

        private void Main_Load(object sender, EventArgs e)
        {

            previousReportageBox.Enabled = !string.IsNullOrEmpty(Program.PreviousReportagePrefix);

            PopulateLibraryList();

            PopulateListBox(((LibraryInfo)LibraryListBox.Items[0]).Id);

            //storage = rep.GetReportageList();

            //ReportagesListBox.DataSource = storage;

            //ReportagesListBox.DisplayMember = "DisplayName";
            //ReportagesListBox.ValueMember = "Reference";

            previousReportageBox.Enabled = !string.IsNullOrEmpty(Program.PreviousReportagePrefix);

            if (Program.PreviousReportagePrefix == null) 
            {
                NewReportageBox.Checked = true;
                previousReportageBox.Enabled = false;
                NewReportageBoxSelected();
            }
            else
            {
                previousReportageBox.Enabled = true;
                previousReportageBox.Checked = true;
                PreviousReportageBoxSelected();
            }

                
        }

        protected void PopulateLibraryList()
        {
            //List<ILibrary> list = new List<ILibrary>();
            //list.Add(new Library { 
            //    Id = Config.LibraryId_Lenta.Value, 
            //    Name = Config.LibraryId_Lenta.Key,
            //    ReportagePath = Config.VisibleReportageFolder,
            //    HiddenReportagePath = Config.HiddenReportageFolder,
            //    NoReportagePath = Config.NoReportageFolder,
            //    XmlPath = Config.XMLFolder
            //});
            //list.Add(new Library { 
            //    Id = Config.LibraryId_Stock.Value, 
            //    Name = Config.LibraryId_Stock.Key,
            //    ReportagePath = Config.StockVisibleReportageFolder,
            //    HiddenReportagePath = Config.StockHiddenReportageFolder,
            //    NoReportagePath = Config.StockNoReportageFolder,
            //    XmlPath = Config.StockXMLFolder
            //});

            LibraryListBox.DataSource = Program.Libraries;

            LibraryListBox.SelectedIndex = 0;
        }

        protected void PopulateListBox(int librarId)
        {
            IReportageRepository rep = new ReportageRepository();

            ReportagesListBox.DataSource = rep.GetReportageList(librarId);

            ReportagesListBox.DisplayMember = "DisplayName";
        }

        private void ReportagesListBoxSelected()
        {
            if (ReportagesListBox.SelectedValue != null)
            {
                NoReportageBox.Checked = false;
                NewReportageBox.Checked = false;
                VisibilityCheckBox.Enabled = false;
                previousReportageBox.Checked = false;
                PreviewBox.Load(((IReportage)ReportagesListBox.SelectedValue).ImageUrl);
            }
        }

        private void NewReportageBoxSelected()
        {
            if (NewReportageBox.Checked)
            {
                NewReportageBox.Checked = true;
                NoReportageBox.Checked = false;
                previousReportageBox.Checked = false;
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
                previousReportageBox.Checked = false;
                ReportagesListBox.SelectedItem = null;
                PreviewBox.Image = null;
                VisibilityCheckBox.Enabled = false;
            }
        }

        private void PreviousReportageBoxSelected()
        {
            if (previousReportageBox.Checked)
            {
                NewReportageBox.Checked = false;
                NoReportageBox.Checked = false;
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
            ProgressBar.Value = 0;

            if (Program.Files != null && Program.Files.Count > 0)
            {
                string prefix = "";
                IProductionMethod methodToProduce = null;
                //methodToProduce.XMLFolder = Config.XMLFolder;

                LibraryInfo selectedLibrary = LibraryListBox.SelectedValue as LibraryInfo;

                if (NoReportageBox.Checked)
                {
                    //methodToProduce = new NoReportageMethod();
                    methodToProduce = new ProductionMethod(selectedLibrary.PicturesPath, selectedLibrary.XmlPath, null);
                }
                else
                    if (NewReportageBox.Checked)
                    {
                        string firstFileInList = Program.Files[0];

                        //if (File.Exists(firstFileInList))
                        //{
                        //    File
                        //}
                        //

                        Random rnd = new Random();

                        prefix = String.Format("{0:yyyyMMdd-HHmmss}-{1}", DateTime.Now, rnd.Next(1000, 10000));

                        if (VisibilityCheckBox.Checked)
                        {
                            methodToProduce = new ProductionMethod(selectedLibrary.ReportagePath, selectedLibrary.XmlPath, prefix);
                        }
                        else
                            methodToProduce = new ProductionMethod(selectedLibrary.ReportageZeroPath, selectedLibrary.XmlPath, prefix);
                    }
                    else
                        if (previousReportageBox.Checked)
                        {
                            prefix = Program.PreviousReportagePrefix;
                            methodToProduce = new ProductionMethod(selectedLibrary.ReportagePath, selectedLibrary.XmlPath, prefix);
                        }
                        else if (ReportagesListBox.SelectedValue != null)
                        {
                            if ((ReportagesListBox.SelectedValue as IReportage) != null)
                            {
                                prefix = ((IReportage)ReportagesListBox.SelectedValue).Reference + "_";

                                methodToProduce = new ProductionMethod(selectedLibrary.ReportagePath, selectedLibrary.XmlPath, prefix);
                                //methodToProduce = new VisibleReportageMethod();
                            }
                        }                        

                List<string> errors = new List<string>();
                int count = 0;
                if (methodToProduce != null && Program.Files != null && Program.Files.Count > 0)
                {

                    if (Config.ReportageDelimeter != null && Config.ReportageDelimeter.Length > 0)
                        methodToProduce.ReportageDelimeter = Config.ReportageDelimeter;

                    OkButton.Enabled = false;

                    Program.SetPrefix(Program.Files[0], prefix);

                    foreach (string file in Program.Files)
                    {
                        if (!methodToProduce.Produce(file, prefix))
                        {
                            errors.Add(file);
                        }
                        count++;
                        ProgressBar.Value = (int)(((double)count / Program.Files.Count) * 100);
                        //ProgressBar.Value += (int)((1.0 / Program.Files.Count) * 100);
                        ProgressBar.Refresh();
                    }

                        ProgressBar.Value = 100;
                        ProgressBar.Refresh();
                }                
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportagesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = (sender as ListBox).IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                OkButton_Click(sender, e);
            }
        }

        private void LibraryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LibraryListBox.SelectedValue != null)
            {
                LibraryInfo selectedItem = (LibraryInfo)LibraryListBox.SelectedValue;

                //PopulateListBox(((ILibrary)LibraryListBox.SelectedValue).Id);
                PopulateListBox(selectedItem.Id);

                this.NoReportageBox.Enabled = !string.IsNullOrEmpty(selectedItem.PicturesPath);

                this.VisibilityCheckBox.Enabled = !string.IsNullOrEmpty(selectedItem.ReportageZeroPath);

                if (this.previousReportageBox.Enabled)
                {
                    previousReportageBox.Checked = true;
                    PreviousReportageBoxSelected();
                }
                else
                {
                    NewReportageBox.Checked = true;
                    NewReportageBoxSelected();
                }
            }
        }

        private void previousReportageBox_CheckedChanged(object sender, EventArgs e)
        {
            PreviousReportageBoxSelected();
        }
    }
}
