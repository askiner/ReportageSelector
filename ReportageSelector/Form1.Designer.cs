namespace ReportageSelector
{
    partial class Form1
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
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportagesListBox
            // 
            this.ReportagesListBox.FormattingEnabled = true;
            this.ReportagesListBox.Location = new System.Drawing.Point(10, 27);
            this.ReportagesListBox.Name = "ReportagesListBox";
            this.ReportagesListBox.Size = new System.Drawing.Size(493, 251);
            this.ReportagesListBox.TabIndex = 0;
            // 
            // ReportagesListBoxLabel
            // 
            this.ReportagesListBoxLabel.AutoSize = true;
            this.ReportagesListBoxLabel.Location = new System.Drawing.Point(10, 10);
            this.ReportagesListBoxLabel.Name = "ReportagesListBoxLabel";
            this.ReportagesListBoxLabel.Size = new System.Drawing.Size(112, 13);
            this.ReportagesListBoxLabel.TabIndex = 0;
            this.ReportagesListBoxLabel.Text = "Выберите репортаж:";
            // 
            // PreviewBox
            // 
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
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "В этот репортаж";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(10, 290);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(98, 28);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Не выпускать";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 332);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.PreviewBox);
            this.Controls.Add(this.ReportagesListBoxLabel);
            this.Controls.Add(this.ReportagesListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ReportagesListBox;
        private System.Windows.Forms.Label ReportagesListBoxLabel;
        private System.Windows.Forms.PictureBox PreviewBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}

