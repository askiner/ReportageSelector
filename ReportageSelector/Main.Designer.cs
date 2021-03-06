﻿namespace ReportageSelector
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ReportagesListBox = new System.Windows.Forms.ListBox();
            this.ReportagesListBoxLabel = new System.Windows.Forms.Label();
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.NewReportageBox = new System.Windows.Forms.RadioButton();
            this.NoReportageBox = new System.Windows.Forms.RadioButton();
            this.VisibilityCheckBox = new System.Windows.Forms.CheckBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.LibraryListBox = new System.Windows.Forms.ComboBox();
            this.previousReportageBox = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportagesListBox
            // 
            this.ReportagesListBox.FormattingEnabled = true;
            this.ReportagesListBox.Location = new System.Drawing.Point(10, 104);
            this.ReportagesListBox.Name = "ReportagesListBox";
            this.ReportagesListBox.Size = new System.Drawing.Size(493, 173);
            this.ReportagesListBox.TabIndex = 5;
            this.ReportagesListBox.SelectedIndexChanged += new System.EventHandler(this.ReportagesListBox_SelectedIndexChanged);
            this.ReportagesListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ReportagesListBox_MouseDoubleClick);
            // 
            // ReportagesListBoxLabel
            // 
            this.ReportagesListBoxLabel.AutoSize = true;
            this.ReportagesListBoxLabel.Location = new System.Drawing.Point(10, 10);
            this.ReportagesListBoxLabel.Name = "ReportagesListBoxLabel";
            this.ReportagesListBoxLabel.Size = new System.Drawing.Size(79, 13);
            this.ReportagesListBoxLabel.TabIndex = 0;
            this.ReportagesListBoxLabel.Text = "Раздел сайта:";
            // 
            // PreviewBox
            // 
            this.PreviewBox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.PreviewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreviewBox.Location = new System.Drawing.Point(510, 27);
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.Size = new System.Drawing.Size(250, 250);
            this.PreviewBox.TabIndex = 2;
            this.PreviewBox.TabStop = false;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(510, 283);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(250, 42);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "Выпуск";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(10, 290);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(98, 28);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Не выпускать";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NewReportageBox
            // 
            this.NewReportageBox.AutoSize = true;
            this.NewReportageBox.Location = new System.Drawing.Point(10, 78);
            this.NewReportageBox.Name = "NewReportageBox";
            this.NewReportageBox.Size = new System.Drawing.Size(111, 17);
            this.NewReportageBox.TabIndex = 3;
            this.NewReportageBox.TabStop = true;
            this.NewReportageBox.Text = "Новый репортаж";
            this.NewReportageBox.UseVisualStyleBackColor = true;
            this.NewReportageBox.CheckedChanged += new System.EventHandler(this.NewReportageBox_CheckedChanged);
            // 
            // NoReportageBox
            // 
            this.NoReportageBox.AutoSize = true;
            this.NoReportageBox.Location = new System.Drawing.Point(10, 56);
            this.NoReportageBox.Name = "NoReportageBox";
            this.NoReportageBox.Size = new System.Drawing.Size(150, 17);
            this.NoReportageBox.TabIndex = 2;
            this.NoReportageBox.TabStop = true;
            this.NoReportageBox.Text = "Не собирать в репортаж";
            this.NoReportageBox.UseVisualStyleBackColor = true;
            this.NoReportageBox.CheckedChanged += new System.EventHandler(this.NoReportageBox_CheckedChanged);
            // 
            // VisibilityCheckBox
            // 
            this.VisibilityCheckBox.AutoSize = true;
            this.VisibilityCheckBox.Checked = true;
            this.VisibilityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.VisibilityCheckBox.Location = new System.Drawing.Point(152, 79);
            this.VisibilityCheckBox.Name = "VisibilityCheckBox";
            this.VisibilityCheckBox.Size = new System.Drawing.Size(135, 17);
            this.VisibilityCheckBox.TabIndex = 4;
            this.VisibilityCheckBox.Text = "Отображать на сайте";
            this.VisibilityCheckBox.UseVisualStyleBackColor = true;
            // 
            // ProgressBar
            // 
            this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgressBar.Location = new System.Drawing.Point(0, 336);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(772, 23);
            this.ProgressBar.Step = 1;
            this.ProgressBar.TabIndex = 6;
            // 
            // LibraryListBox
            // 
            this.LibraryListBox.DisplayMember = "Name";
            this.LibraryListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LibraryListBox.FormattingEnabled = true;
            this.LibraryListBox.Location = new System.Drawing.Point(93, 7);
            this.LibraryListBox.Name = "LibraryListBox";
            this.LibraryListBox.Size = new System.Drawing.Size(121, 21);
            this.LibraryListBox.TabIndex = 1;
            this.LibraryListBox.SelectedIndexChanged += new System.EventHandler(this.LibraryListBox_SelectedIndexChanged);
            // 
            // previousReportageBox
            // 
            this.previousReportageBox.AutoSize = true;
            this.previousReportageBox.Location = new System.Drawing.Point(10, 34);
            this.previousReportageBox.Name = "previousReportageBox";
            this.previousReportageBox.Size = new System.Drawing.Size(143, 17);
            this.previousReportageBox.TabIndex = 8;
            this.previousReportageBox.TabStop = true;
            this.previousReportageBox.Text = "Предыдущий репортаж";
            this.previousReportageBox.UseVisualStyleBackColor = true;
            this.previousReportageBox.CheckedChanged += new System.EventHandler(this.previousReportageBox_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 359);
            this.Controls.Add(this.previousReportageBox);
            this.Controls.Add(this.LibraryListBox);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.VisibilityCheckBox);
            this.Controls.Add(this.NoReportageBox);
            this.Controls.Add(this.NewReportageBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.ReportagesListBoxLabel);
            this.Controls.Add(this.ReportagesListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Выпуск фото: выбор репортажа";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ReportagesListBox;
        private System.Windows.Forms.Label ReportagesListBoxLabel;
        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.Button OkButton;
        new private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.RadioButton NewReportageBox;
        private System.Windows.Forms.RadioButton NoReportageBox;
        private System.Windows.Forms.CheckBox VisibilityCheckBox;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ComboBox LibraryListBox;
        private System.Windows.Forms.RadioButton previousReportageBox;
    }
}

